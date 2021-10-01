using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryCatalog.Data;
using LibraryCatalog.Models;
using LibraryCatalog.Interfaces;
using Microsoft.AspNetCore.Identity;
using LibraryCatalog.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace LibraryCatalog.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReview _review;
        private readonly IBook _book;
        private readonly IUser _user;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewController(IReview review, IBook book, IUser user, UserManager<ApplicationUser> userManager)
        {
            _review = review;
            _book = book;
            _user = user;
            _userManager = userManager;
        }

        public ViewResult List(int id)
        {

            List<Review> reviews;
            reviews = _review.GetReviewsByBookId(id).ToList();
            var book = _book.GetBookById(id);
           
            return View(new ReviewsViewModel
            {
                GetReviews = reviews,
                BookDetails = book
            });
        }

        // GET: Review
        public ActionResult Index(int id)
        {
            List(id);
            return View();
        }

        // GET: Review/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = _review.GetReviewById(Convert.ToInt32(id));
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Review/Create
        public IActionResult Create(int? id)
        {
            if(id != null)
                ViewBag.BookId = Convert.ToInt32(id);

            return View();
        }

        // POST: Review/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ReviewId,Comment,Rating,UserId,BookId")] BooksViewModel review)
        {
            if (ModelState.IsValid)
            {
                review.UserId = _userManager.GetUserId(User);
                var user = _user.GetUserById(review.UserId);

                if(user.Status != UserStatus.Active)
                {
                    TempData["message"] = "Review not submitted, because your account has been blocked.";
                    return RedirectToAction("Index", new { id = (review.BookId).ToString() });
                }

                var checkReview = _review.GetReviewsByUserId(review.UserId).FirstOrDefault(b => b.BookId == review.BookId);

                if(checkReview == null)
                {
                      var newReview = new Review();
                      {
                          newReview.BookId = review.BookId;
                          newReview.UserId = review.UserId;
                          newReview.ReviewDate = System.DateTime.Now;
                          newReview.Comment = review.Comment;
                          newReview.Rating = review.Rating;
                      }
                    
                    _review.Add(newReview);
                    _review.Save();
                    TempData["message"] = "Book review submitted";
                }
                else
                {
                    TempData["message"] = "You can only review a book once.";
                }
            }

            return RedirectToAction("Details","Book", new { id = review.BookId.ToString() });
        }

        // GET: Review/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = _review.GetReviewById(Convert.ToInt32(id));
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Review/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewId,Comment,Rating,UserId,BookId")] Review review)
        {
            if (id != review.ReviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    _review.Edit(review);
                    _review.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_review.ReviewExists(review.ReviewId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        // GET: Review/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = _review.GetReviewById(Convert.ToInt32(id));
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Review/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var review = _review.GetReviewById(id);
            _review.Delete(review);
            _review.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
