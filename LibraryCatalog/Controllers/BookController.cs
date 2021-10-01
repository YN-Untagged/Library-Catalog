using LibraryCatalog.DAL;
using LibraryCatalog.Models;
using LibraryCatalog.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private readonly LibraryDbContext _context;
        //private readonly IHostingEnvironment hostingEnvironment;
        private readonly IWebHostEnvironment hostEnvironment;

        public BookController(LibraryDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this.hostEnvironment = hostEnvironment;
        }

        public ViewResult List(string genre = null)
        {
            //System.DateTime date = new DateTime(1999);
            //IEnumerable<Book> books;
            List<BookAuthorViewModel> books;
            //IEnumerable<Book> popular;
            IEnumerable<Book> latest;
            if (genre != null)
            {
                //books = _context.Books.Where(b => b.Genre == genre).ToList();
                books = (from b in _context.Books
                        join ba in _context.BookAuthors
                        on b.BookId equals ba.BookId
                        join a in _context.Authors
                        on ba.AuthorId equals a.AuthorId
                         where b.Genre == genre
                        select new BookAuthorViewModel
                        {
                            BookId = b.BookId,
                            ISBN = b.ISBN,
                            Title = b.Title,
                            Publisher = b.Publisher,
                            Year = b.Year,
                            URL = b.URL,
                            Name = a.Name,
                            Surname = a.Surname,
                            Genre = b.Genre
                        } ).ToList();
            }
            else
            {
                books = (from b in _context.Books
                         join ba in _context.BookAuthors
                                 on b.BookId equals ba.BookId
                         join a in _context.Authors
                         on ba.AuthorId equals a.AuthorId
                         select new BookAuthorViewModel
                         {
                             BookId = b.BookId,
                             ISBN = b.ISBN,
                             Title = b.Title,
                             Publisher = b.Publisher,
                             Year = b.Year,
                             URL = b.URL,
                             Name = a.Name,
                             Surname = a.Surname,
                             Genre = b.Genre
                         }).ToList();
            }

            
            //popular = _context.Books.ToList();
            //latest = _context.Books.Where(b => b.Year >= (Convert.ToInt32(date)-2 )).ToList();
            latest = _context.Books.Where(b => b.Year >= 2000).ToList();

            return View(new BooksViewModel
            {
                //NewBooks = latest.OrderByDescending(y => y.Year),
                //AllBooks = books
            });
        }


        public IActionResult Index(string genre)
        {
            List<BookAuthorViewModel> books;
            
            if (genre != null)
            {
                //Book of specific genre
                books = (from b in _context.Books
                         join ba in _context.BookAuthors
                         on b.BookId equals ba.BookId
                         join a in _context.Authors
                         on ba.AuthorId equals a.AuthorId
                         where b.Genre == genre
                         select new BookAuthorViewModel
                         {
                             BookId = b.BookId,
                             ISBN = b.ISBN,
                             Title = b.Title,
                             Publisher = b.Publisher,
                             Year = b.Year,
                             URL = b.URL,
                             Name = a.Name,
                             Surname = a.Surname,
                             Genre = b.Genre
                         }).ToList();
            }
            else
            {
                //Get all Books
                /*books = (from b in _context.Books
                         join ba in _context.BookAuthors
                         on b.BookId equals ba.BookId
                         join a in _context.Authors
                         on ba.AuthorId equals a.AuthorId
                         select new BookAuthorViewModel
                         {
                             BookId = b.BookId,
                             ISBN = b.ISBN,
                             Title = b.Title,
                             Publisher = b.Publisher,
                             Year = b.Year,
                             URL = b.URL,
                             Name = a.Name,
                             Surname = a.Surname,
                             Genre = b.Genre
                         }).ToList();*/
                books = (from b in _context.Books
                         join ba in _context.BookAuthors
                         on b.BookId equals ba.BookId
                         join a in _context.Authors
                         on ba.AuthorId equals a.AuthorId
                         select new BookAuthorViewModel
                         {
                             BookId = b.BookId,
                             ISBN = b.ISBN,
                             Title = b.Title,
                             Publisher = b.Publisher,
                             Year = b.Year,
                             URL = b.URL,
                             Name = a.Name,
                             Surname = a.Surname,
                             Genre = b.Genre
                         }).ToList();
            }

            return View(books);
        }

        

        // Get book details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET (Create Book)
        public IActionResult Create()
        {
            var authors = _context.Authors;
            ViewData["authors"] = authors;//new List<Author> authors(_context.Authors, "AuthorId", ("Name" +" "+ "Surname"));


            return View(new BooksViewModel
            {
                Authors = authors
            }) ;
        }

        // POST (Create Book)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BooksViewModel bookVM)
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
                book.URL = fileName;
                book.Genre = bookVM.Genre;
                
            }
            _context.Add(book);
            _context.SaveChanges();
            var bookId = book.BookId;

            if (bookId != 0)
            {
                var author = (from a in _context.Authors
                             where a.Name == bookVM.AuthorName
                             where a.Surname == bookVM.AuthorLastName
                             select a.AuthorId).FirstOrDefault();
                if (author != 0)
                {
                    var authorId = author;
                    AddBookAuthor(authorId, bookId);
                }
                else
                {
                    AddAuthor(bookVM.AuthorName, bookVM.AuthorLastName, bookId);
                }
            }

            return RedirectToAction("Details", new { id = bookId.ToString()});
        }

        private void AddAuthor(string authorName, string authorLastName, int bookId)
        {
            var author = new Author();
            {
                author.Name = authorName;
                author.Surname = authorLastName;
            }
            _context.Add(author);
            _context.SaveChanges();
            var authorId = author.AuthorId;
            AddBookAuthor(authorId, bookId);
        }

        private void AddBookAuthor(int authorId, int bookId)
        {
            var bookAuthor = new BookAuthor();
            {
                bookAuthor.AuthorId = authorId;
                bookAuthor.BookId = bookId;
            }
            _context.Add(bookAuthor);
            _context.SaveChanges();
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
                       
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,IFormFile BookCover,[Bind("BookId,ISBN,Title,Pages,Genre,Year,URL,Publisher")] Book book)
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
                    book.URL = Guid.NewGuid().ToString() + "_" + BookCover.FileName;
                    string filePath = Path.Combine(UploadFolder, book.URL);
                    BookCover.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                try
                {
                    
                    _context.Update(book);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        

        /*// GET (Edit book)
        public ViewResult GetBook(int? id)
        {
            Book books;
            books = _context.Books.Find(id);

            return View(new EditBookViewModel
            {
                GetBookById = books
            }) ;
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //GetBook(id);
            Book book;
            //BooksViewModel book = new BooksViewModel();
            book = _context.Books.Find(id);
             /*var book = (from b in _context.Books
                    where b.BookId == id
                    select new BooksViewModel
                    {
                        BookId = b.BookId,
                        Title = b.Title,
                        ISBN = b.ISBN,
                        Pages = b.Pages,
                        Genre = b.Genre,
                        URL = b.URL,
                        Publisher = b.Publisher,
                        Year = b.Year
                    });

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditBookViewModel book)
        {
            if (id == 0) //book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string fileName = null;

                if (book.BookCover != null)
                {
                    string UploadFolder = Path.Combine(hostEnvironment.WebRootPath + "/images");
                    fileName = Guid.NewGuid().ToString() + "_" + book.BookCover.FileName;
                    string filePath = Path.Combine(UploadFolder, fileName);
                    book.BookCover.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                try
                {
                    var b = new Book();
                    {
                        b.BookId = id;
                        b.ISBN = book.ISBN;
                        b.Title = book.Title;
                        b.Pages = book.Pages;
                        b.Genre = book.Genre;
                        b.Year = book.Year;
                        b.Publisher = book.Publisher;
                        if (fileName != null)
                        {
                            b.URL = fileName;
                        }
                    }
                    
                    _context.Update(b);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Detail", new { id = id.ToString() });
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
            }
            return View(book);
        }*/

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }

    }
}
