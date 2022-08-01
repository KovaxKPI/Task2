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
        public async Task<IEnumerable<BookDto>> Get(string genre)
        {
            var books = from b in _context.Books.Include(x => x.ratings)
                        .Include(y => y.reviews)
                        .Where(x => x.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                        select new BookDto()
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            Rating = b.ratings.Average(x => x.Score),
                            reviewsNumber = b.reviews.Count()
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
