using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.ViewModels
{
    public class BookAuthorViewModel
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public int Pages { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Publisher { get; set; }
        public string URL { get; set; }
        public string Genre { get; set; }
        public IFormFile BookCover { get; set; }
    }
}
