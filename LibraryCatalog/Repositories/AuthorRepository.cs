using LibraryCatalog.Data;
using LibraryCatalog.Interfaces;
using LibraryCatalog.Models;
using LibraryCatalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Repositories
{
    public class AuthorRepository:  IAuthor
    {
        private readonly LibraryDbContext _context;
        public AuthorRepository(LibraryDbContext context)
        {
            _context = context;
        }


        public void Add(Author author)
        {
            _context.Authors.Add(author);
        }

        public void Delete(Author author)
        {
            _context.Authors.Remove(author);
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _context.Authors;
        }

        public Author GetAuthorById(int id)
        {
            return _context.Authors.Where(a => a.AuthorId == id).FirstOrDefault();
        }
        public void Edit(Author author)
        {
            _context.Authors.Update(author);
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public List<BookAuthorViewModel> GetBooks()
        {
            var authorBooks = (from b in _context.Books
                               join ba in _context.BookAuthors
                               on b.BookId equals ba.BookId
                               select new BookAuthorViewModel
                               {
                                   Title = b.Title,
                                   BookId = ba.BookId,
                                   AuthorId = ba.AuthorId
                               }).ToList();
            return authorBooks;
        }

        public void AddBookAuthor(BookAuthor bookAuthor)
        {
            _context.BookAuthors.Add(bookAuthor);
        }

        public bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.AuthorId == id);
        }
    }
}