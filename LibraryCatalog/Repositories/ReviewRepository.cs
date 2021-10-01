using LibraryCatalog.Data;
using LibraryCatalog.Interfaces;
using LibraryCatalog.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Repositories
{
    public class ReviewRepository : IReview
    {
        private readonly LibraryDbContext _context;
        public ReviewRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public void Delete(Review review)
        {
            _context.Reviews.Remove(review);
        }
        public void Add(Review review)
        {
            _context.Reviews.Add(review);
        }
        public void Edit(Review review)
        {
            _context.Reviews.Update(review);
        }

        public List<Review> GetReviews()
        {
            return _context.Reviews.OrderByDescending(r => r.ReviewId).Include(b => b.ApplicationUser).Include(b => b.Book).ToList();
        }

        public IEnumerable<Review> GetReviewsByBookId(int id)
        {
            return _context.Reviews.Where(b => b.BookId == id).OrderByDescending(r => r.ReviewId).Include(b => b.Book).Include(u => u.ApplicationUser);
        }

        public IEnumerable<Review> GetReviewsByUserId(string id)
        {
            return _context.Reviews.Where(u => u.UserId == id).OrderByDescending(r => r.ReviewId).Include(b => b.Book);
        }
        public Review GetReviewById(int id)
        {
            return _context.Reviews.Find(id);
        }

        public int[,] GetRatings()
        {
            var query = GetReviews().GroupBy(
                            bookId => bookId.BookId,
                            rating => rating.Rating,
                            (bookId, rating) => new
                            {
                                Key = bookId,
                                Average = rating.Average()
                            });
            
            int rows = query.Count();
            int cols = 2;
            int[,] ratings = new int[rows, cols];

            for (int i =0; i < rows; i++)
            {
                ratings[i,0] = query.ElementAt(i).Key;
                ratings[i,1] = Convert.ToInt32(query.ElementAt(i).Average);
            }

            return ratings;
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public bool ReviewExists(int id)
        {
            return _context.Reviews.Any(r => r.ReviewId == id);
        }
    }
}
