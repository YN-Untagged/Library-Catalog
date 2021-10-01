using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryCatalog.Data;
using LibraryCatalog.Models;
using LibraryCatalog.ViewModels;
using Microsoft.AspNetCore.Identity;
using LibraryCatalog.Interfaces;

namespace LibraryCatalog.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthor _author;
        private readonly IBook _book;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthorController(IAuthor author, IBook book, UserManager<ApplicationUser> userManager)
        {
            _author = author;
            _book = book;
            _userManager = userManager;
        }

        public ViewResult List()
        {
            IEnumerable<Author> authors;
            var authorBooks = _book.GetBookAuthors();
            authors = _author.GetAuthors();

            return View(new BooksViewModel
            {
                Authors = authors,
                BooksByAuthor = authorBooks,
            });
        }

        // GET: Author
        public IActionResult Index()
        {
            List();
            return View();
        }

        // GET: Author/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _author.GetAuthorById(Convert.ToInt32(id));
            var  books = _book.GetBookAuthors().Where(a => a.AuthorId == id).ToList();
            if (author == null)
            {
                return NotFound();
            }

            ViewBag.books = books;

            return View(author);
        }

        // GET: Author/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Author/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("AuthorId,FullName,Biography")] Author author)
        {
            if (ModelState.IsValid)
            {
                author.CreatedBy = _userManager.GetUserId(User);
                author.CreateDateTime = System.DateTime.Now;
                _author.Add(author);
                _author.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Author/Edit
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _author.GetAuthorById(Convert.ToInt32(id));
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Author/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,FullName,Biography")] Author author)
        {
            if (id != author.AuthorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    author.EditedBy = _userManager.GetUserId(User);
                    author.EditedDate = System.DateTime.Now;
                    _author.Edit(author);
                    _author.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.AuthorId))
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
            return View(author);
        }

        // GET: Author/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = _author.GetAuthorById(Convert.ToInt32(id));
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Author/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var author = _author.GetAuthorById(id);
            _author.Delete(author);
            _author.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _author.AuthorExists(id);
        }
    }
}
