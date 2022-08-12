using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task2.Models;

namespace Task2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendedController : ControllerBase
    {
        private ApiContext _context;
        public RecommendedController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<BookDto> Get(string genre)
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
            var rec = new List<BookDto>();
            foreach(var book in books.OrderByDescending(x => x.Rating))
            {
                if (book.reviewsNumber >= 10)
                    rec.Add(book);
            }
            return rec;
        }
    }
}
