using LibraryCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.ViewModels
{
    public class ReviewsViewModel
    {
        public IEnumerable<Review> GetReviews { get; set; }
        public Book BookDetails { get; set; }
        public int ReviewId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
    }
}
