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
        public string AuthorName { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
    }
}
