using LibraryCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Interfaces
{
    public interface IReview
    {
        List<Review> GetReviews();
        IEnumerable<Review> GetReviewsByUserId(string id);
        IEnumerable<Review> GetReviewsByBookId(int id);
        Review GetReviewById(int id);
        //IQueryable<Review> GetRatings();
        int[,] GetRatings();
        void Delete(Review review);
        void Add(Review review);
        void Edit(Review review);
        bool ReviewExists(int id);
        void Save();

    }
}
