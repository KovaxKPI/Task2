using Microsoft.EntityFrameworkCore;
using Task2.EF;
using Task2.Interfaces;
using Task2.Models;

namespace Task2.Services
{
    public class RecommendedService : IRecommendedService
    {
        private readonly ApiContext _context;
        public RecommendedService(ApiContext apiContext)
        {
            _context = apiContext;
        }
        public IQueryable<BookDto> Get(string genre)
        {
            var books = from b in _context.Books.Include(x => x.Ratings)
                        .Include(y => y.Reviews)
                        .Where(x => x.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                        select new BookDto()
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            Rating = b.Ratings == null || b.Ratings.Count == 0 ? 0 : b.Ratings.Average(x => x.Score),
                            reviewsNumber = b.Reviews == null || b.Reviews.Count == 0 ? 0 : b.Reviews.Count()
                        };
            return books;
        }
    }
}
