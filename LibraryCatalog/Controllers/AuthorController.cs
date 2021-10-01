using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryCatalog.DAL;
using LibraryCatalog.Models;
using LibraryCatalog.ViewModels;

namespace LibraryCatalog.Controllers
{
    public class AuthorController : Controller
    {
        private readonly LibraryDbContext _context;

        public AuthorController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Author
        public IActionResult Index()
        {
            var result = (from ba in _context.BookAuthors
                          join a in _context.Authors
                          on ba.AuthorId equals a.AuthorId
                          join b in _context.Books
                          on ba.BookId equals b.BookId
                          select new BookAuthorViewModel
                          {
                              AuthorId = a.AuthorId,
                              Name = a.Name,
                              Surname = a.Surname,
                              BookId = b.BookId,
                              Title = b.Title
                          }).ToList();

            return View(result);
        }

        // GET: Author/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = (from ba in _context.BookAuthors
                          join a in _context.Authors
                          on ba.AuthorId equals a.AuthorId
                          join b in _context.Books
                          on ba.BookId equals b.BookId
                          select new BookAuthorViewModel
                          {
                              AuthorId = a.AuthorId,
                              Name = a.Name,
                              Surname = a.Surname,
                              BookId = b.BookId,
                              Title = b.Title
                          }).FirstOrDefault(a => a.AuthorId == id);

            //var author = await _context.Authors.FirstOrDefault(m => m.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Create([Bind("AuthorId,Name,Surname")] Author author)
        {
            if (ModelState.IsValid)
            {
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Author/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Author/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,Name,Surname")] Author author)
        {
            if (id != author.AuthorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(author);
                    await _context.SaveChangesAsync();
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

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Author/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.AuthorId == id);
        }
    }
}
