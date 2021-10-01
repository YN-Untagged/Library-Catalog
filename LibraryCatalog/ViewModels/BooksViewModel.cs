using LibraryCatalog.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.ViewModels
{
    public class BooksViewModel
    {
        public IEnumerable<Book> AllBooks { get; set; }
        public IEnumerable<Book> PopularBooks { get; set; }
        public IEnumerable<Book> NewBooks { get; set; }
        public IQueryable<Book> GetBooks { get; set; }
        public IEnumerable<Author> Authors { get; set; }

        public string[] newAuthor { get; set; } 
        public int[] selectedAuthors { get; set; }

        public int BookId {get; set;}
        public int Year { get; set; }
        public string ISBN { get; set; }

        [Display(Name = "Number of Pages")]
        public int Pages { get; set; }
        public string Publisher { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public string Genre { get; set; }

        public string AuthorId { get; set; }

        [Display(Name = "First Name(s)")]
        public string AuthorName { get; set; }

        [Display(Name = "Last Name")]
        public string AuthorLastName { get; set; }

        
        [FileExtensions(Extensions = "jpg, png", ErrorMessage = "File extenion must only be *.jpg or *.png")]
        public IFormFile BookCover { get; set; }
    }
}
