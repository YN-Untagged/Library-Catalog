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
    public class LoanRepository : ILoan
    {
        public readonly LibraryDbContext _context;
        public LoanRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public int MostLoanedBook()
        {
            var query = GetBookLoans().GroupBy(
                            bookId => bookId.BookId,
                            bookloan => bookloan.BookLoanId,
                            (bookId, bookloan) => new
                            {
                                Key = bookId,
                                Count = bookloan.Count()
                            }).OrderByDescending(c => c.Count);
            var best = query.FirstOrDefault();
            return best.Key;
        }
        public void Add(BookLoan bookLoan)
        {
            _context.BookLoans.Add(bookLoan);
        }

        public void Delete(BookLoan bookLoan)
        {
            _context.BookLoans.Remove(bookLoan);
        }

        public List<BookLoan> GetBookLoans()
        {
            return _context.BookLoans.Include(b => b.ApplicationUser).Include(b => b.Book).ToList();
        }

        public BookLoan GetBookLoanById(int id)
        {
            return _context.BookLoans.Include(b => b.ApplicationUser).Include(b => b.Book).FirstOrDefault(b => b.BookLoanId == id);
        }

        public IEnumerable<BookLoan> GetBookLoanByUserId(string id)
        {
            return _context.BookLoans.Include(b => b.ApplicationUser).Include(b => b.Book).Where(b => b.UserId == id);
        }

        public IQueryable<BookLoan> GetBookLoansById(string id)
        {
            return _context.BookLoans.Include(b => b.ApplicationUser).Include(b => b.Book).Where(b => b.UserId == id);
        }

        public void Edit(BookLoan bookLoan)
        {
            _context.BookLoans.Update(bookLoan);
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public bool BookLoanExists(int id)
        {
            return _context.BookLoans.Any(e => e.BookLoanId == id);
        }

        public List<Book> MostLoaned()
        {
            var books = _context.Books.Include(bl => bl.BookLoans).OrderByDescending(b => b.BookLoans.Count).Where(c => c.BookLoans.Count > 0).ToList();
            return books;
        }

        
    }
}
