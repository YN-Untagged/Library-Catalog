using LibraryCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.ViewModels
{
    public class DashBoardViewModel
    {
        

        public IEnumerable<ApplicationUser> GetUsers { get; set; }
        public IEnumerable<ApplicationUser> GetMembers { get; set; }
        public IEnumerable<ApplicationUser>  GetLibrarians { get; set; }
        public IEnumerable<ApplicationUser> GetAdmins { get; set; }
        public IEnumerable<BookLoan> GetLoans { get; set; }
        public IEnumerable<Author> GetAuthors { get; set; }
        public Book MonthBook { get; set; }
        public IEnumerable<Book> GetBooks { get; set; }
        public IEnumerable<BookLoan> CheckedOut { get; set; }
        public IEnumerable<BookLoan> OverDued { get; set; }
        public int AllFemales { get; set; }
        public int AllMales { get; set; }
        public int mFemales { get; set; }
        public int mMales { get; set; }
        public decimal AmountOwed { get; set; }

    }
}
