using LibraryCatalog.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.ViewModels
{
    public class LoanViewModel
    {
        public IEnumerable<BookLoan> GetBookLoans { get; set; }
        public IEnumerable<BookLoan> GetCheckedOut { get; set; }
        public IEnumerable<BookLoan> GetOverDued { get; set; }
        public IEnumerable<BookLoan> GetRequested { get; set; }
        public IEnumerable<Reservation> GetReserved { get; set; }

    }
}
