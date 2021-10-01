using LibraryCatalog.Data;
using LibraryCatalog.Interfaces;
using LibraryCatalog.Models;
using LibraryCatalog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Controllers
{
    public class BookController : Controller
    {
        private readonly IBook _book;
        private readonly IReview _review;
        private readonly ILoan _loan;
        private readonly IAuthor _author;
        private readonly UserManager <ApplicationUser> _userManager;
        private readonly IWebHostEnvironment hostEnvironment;

        public BookController( IBook book, IReview review, ILoan loan, IAuthor author, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _loan = loan;
            _book = book;
            _review = review;
            _author = author;
            _userManager = userManager;
            this.hostEnvironment = hostEnvironment;
        }

        public static IEnumerable<SelectListItem> GetSelectLists<T>()
        {
            return (Enum.GetValues(typeof(T)).Cast<T>().Select(
                enumList => new SelectListItem() { Text = enumList.ToString(), Value = enumList.ToString() })).ToList();
        }

        public ViewResult List(string genre)
        {
          
            List<Book> books;
            if (genre != null)
            {
                books = _book.GetBooks().Where(b => b.Genre == genre).ToList();
            }
            else
            {
                books = _book.GetBooks().ToList();
            }

            var latest = _book.GetBooks().Where(y => y.Year >= System.DateTime.Now.Year - 1);
            var popular = _loan.MostLoaned();
            var authors = _book.GetAuthors();
            var reviews = _review.GetRatings();

            return View(new BooksViewModel
            {
                AllBooks = books,
                BookAuthors = authors,
                GetBooksRating = reviews,
                PopularBooks = popular,
                NewBooks = latest
            });
        }
        public ViewResult List(int id)
        {
            var book = _book.GetBookById(id);
            var reviews = _review.GetReviewsByBookId(id).OrderByDescending(d => d.ReviewDate);
            var average = -1;
            if(reviews.Any())
            {
                average = (int)reviews.Average(r => r.Rating);
            }
            //var rating = _review.GetReviewsByBookId(id).Sum(d => d.Rating) / reviews.Count();
            return View(new BooksViewModel
            {
                GetBook = book,
                GetReviews = reviews,
                averageRating = average
            });
        }

        public IActionResult Index(string genre)
        {
            List(genre);

            return View();
        }
        public IActionResult Main()
        {
            List(null);
            return View();
        }
        

        // Get book details
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                List((int)id);
            }

            ViewBag.Reviewed = _review.GetReviewsByBookId((int)id).Any(u => u.UserId == _userManager.GetUserId(User));

            return View();
        }

        // GET (Create Book)
        [Authorize(Policy = "writepolicy")]
        public IActionResult Create()
        {
            var authors = _author.GetAuthors();
            ViewData["authors"] = authors;
            ViewData["genres"] = GetSelectLists<Genres>();
            ViewData["status"] = GetSelectLists<BookStatus>();


            return View(new BooksViewModel
            {
                Authors = authors
            }) ;
        }

        // POST (Create Book)
        [Authorize(Policy = "writepolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("BookId,ISBN,Title,Pages,Genre,Year,Image,Publisher,Language,Description,Price,InputAuthors, SelectedAuthors,BookCover")] BooksViewModel bookVM)
        {        
            string fileName = null;

            if (bookVM.BookCover != null)
            {
                string UploadFolder = Path.Combine(hostEnvironment.WebRootPath + "/images");
                fileName = Guid.NewGuid().ToString() + "_" + bookVM.BookCover.FileName;
                string filePath = Path.Combine(UploadFolder, fileName);
                bookVM.BookCover.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            else
            {
                fileName = "placeholder.jpg";
            }

            var book = new Book();
            {
                book.ISBN = bookVM.ISBN;
                book.Title = bookVM.Title;
                book.Pages = bookVM.Pages;
                book.Publisher = bookVM.Publisher;
                book.Year = bookVM.Year;
                book.Image = fileName;
                book.Genre = bookVM.Genre;
                book.Language = bookVM.Language;
                book.Price = bookVM.Price;
                book.Status = "Available";
                book.Description = bookVM.Description;
               
            }
            _book.Add(book);
            _book.Save();
            var bookId = book.BookId;

            if (bookId != 0)
            {
                if (bookVM.SelectedAuthors != null)
                {
                    AddBookAuthor(bookVM.SelectedAuthors, bookId);
                }

                for (int i = 0; i < bookVM.InputAuthors.Count(); i++)
                {
                    if(bookVM.InputAuthors[i] != null)
                    {
                        AddAuthor(bookVM.InputAuthors, bookId);
                        break;
                    }
                }
            }

            return RedirectToAction("Details", new { id = bookId.ToString()});
        }

        private void AddAuthor(string[] authors, int bookId)
        {
            //int[] authorIds = { };
            List<int> authorIds = new List<int>();
            for (int a = 0; a < authors.Length; a++)
            {
                if (authors[a] != null)
                {
                    var author = new Author();
                    {
                        author.FullName = authors[a];
                    }
                    _author.Add(author);
                    _author.Save();
                    authorIds.Add(author.AuthorId);
                }
            }
            
            AddBookAuthor(authorIds.ToArray(), bookId);
        }

        private void AddBookAuthor(int[] authors, int bookId)
        {
            for (int i = 0; i < authors.Length; i++)
            {
                var bookAuthor = new BookAuthor();
                {
                    bookAuthor.AuthorId = authors[i];
                    bookAuthor.BookId = bookId ;
                }
                _author.AddBookAuthor(bookAuthor);
            }
            //bookAuthor.ForEach(b => _context.BookAuthors.Add(b));
            _author.Save();
        }

        // GET: Book/Edit/5
        [Authorize(Policy = "writepolicy")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _book.GetBookById(Convert.ToInt32(id));
            ViewData["genres"] = GetSelectLists<Genres>();
            ViewData["status"] = GetSelectLists<BookStatus>();

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [Authorize(Policy = "writepolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,IFormFile BookCover,[Bind("BookId,ISBN,Title,Pages,Genre,Year,Image,Publisher, Status, Price, Language,AmazonUrl,Description,TakeAlotUrl")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (BookCover != null)
                {
                    string UploadFolder = Path.Combine(hostEnvironment.WebRootPath + "/images");
                    book.Image = Guid.NewGuid().ToString() + "_" + BookCover.FileName;
                    string filePath = Path.Combine(UploadFolder, book.Image);
                    BookCover.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                try
                {
                    
                    _book.Edit(book);
                    _book.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Details", new { id = id.ToString() });
            }
            return View(book);
        }


        // GET: Book/Delete/5
        [Authorize(Policy = "writepolicy")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _book.GetBookById(Convert.ToInt32(id));
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [Authorize(Policy = "writepolicy")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _book.GetBookById(id);
            _book.Delete(book);
            _book.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _book.BookExists(id);
        }

    }
}
