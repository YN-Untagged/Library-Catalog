using LibraryCatalog.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.ViewModels
{
    public class EditBookViewModel
    {
        public int BookId { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public int Pages { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public string URL { get; set; }
        public string Publisher { get; set; }

        [FileExtensions(Extensions = "jpg, png", ErrorMessage = "File extenion must only be *.jpg or *.png")]
        public IFormFile BookCover { get; set; }

        public Book GetBookById { get; set; }

    }
}
