using LibraryCatalog.Data;
using LibraryCatalog.Interfaces;
using LibraryCatalog.Models;
using LibraryCatalog.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Repositories
{
    public class BookRepository : IBook
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<BookAuthor> GetBookAuthors()
        {
            return (_context.BookAuthors.Include(b => b.Book).Include(a => a.Author));
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
        }

        public void Delete(Book book)
        {
            _context.Books.Remove(book);
        }

        public List<Book> GetBooks(string genre)
        {
            List<Book> books;
            if (genre != null)
            {
                books = _context.Books.Where(b => b.Genre == genre).Include(r => r.Reviews).ToList();
            }
            else 
            {
                books = _context.Books.Include(r => r.Reviews).ToList();
            }
            return books;
        }

        public Book GetBookById(int id)
        {
            return _context.Books.Where(b => b.BookId == id).FirstOrDefault();
        }
        public void Edit(Book book)
        {
            _context.Books.Update(book);
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public List<BookAuthorViewModel> GetAuthors()
        {
            var bookAuthors = (from a in _context.Authors
                           join ba in _context.BookAuthors
                           on a.AuthorId equals ba.AuthorId
                           select new BookAuthorViewModel
                           {
                               AuthorName = a.FullName,
                               AuthorId = ba.AuthorId,
                               BookId = ba.BookId
                           }).ToList();
            return bookAuthors;
        }

        public bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
