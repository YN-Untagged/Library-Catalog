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
        public IEnumerable<Review> GetReviews { get; set; }
        public Book GetBook { get; set; }
        public int[,] GetBooksRating { get; set; }
        public IEnumerable<BookAuthorViewModel> BookAuthors { get; set; }
        public IEnumerable<BookAuthor> BooksByAuthor { get; set; }
        public Review GetReview { get; set; }
        public int averageRating { get; set; }
        public int BookId {get; set;}
        public int Year { get; set; }
        public string ISBN { get; set; }

        [Display(Name = "Number of Pages")]
        public int Pages { get; set; }
        public string Publisher { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public string Genre { get; set; }
        public string AmazonUrl { get; set; }
        public string Description { get; set; }
        public string TakeAlotUrl { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string biography { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }

        public string[] InputAuthors { get; set; }
        public int[] SelectedAuthors { get; set; }
        
        [FileExtensions(Extensions = "jpg, png", ErrorMessage = "File extenion must only be *.jpg or *.png")]
        public IFormFile BookCover { get; set; }
    }
}
