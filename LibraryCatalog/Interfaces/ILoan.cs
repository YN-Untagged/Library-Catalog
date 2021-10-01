using LibraryCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Interfaces
{
    public interface ILoan
    {
        List<BookLoan> GetBookLoans ();
        int MostLoanedBook();
        void Add(BookLoan bookLoan);
        void Edit(BookLoan bookLoan);
        void Delete(BookLoan bookLoan);
        BookLoan GetBookLoanById(int id);
        IEnumerable<BookLoan> GetBookLoanByUserId(string id);
        IQueryable<BookLoan> GetBookLoansById(string id);
        bool BookLoanExists(int id);
        List<Book> MostLoaned();
        void Save();
        void Dispose();
        
    }
}
