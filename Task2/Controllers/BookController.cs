using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task2.Models;

namespace Task2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private ApiContext _context;
        public BookController(ApiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IEnumerable<BookDto>> GetBooks()
        {
            var books = from b in _context.Books.Include(x => x.ratings)
                        .Include(y => y.reviews)
                        select new BookDto()
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            Rating = b.ratings.Average(x => x.Score),
                            reviewsNumber = b.reviews.Count()
                        };
            return books;
        }
        [HttpGet("{order:alpha}")]
        public async Task<IEnumerable<BookDto>> GetBooksByOrder(string order)
        {
            var books = from b in _context.Books.Include(x => x.ratings)
                        .Include(y => y.reviews)
                        select new BookDto()
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            Rating = b.ratings.Average(x => x.Score),
                            reviewsNumber = b.reviews.Count()
                        };
            if (order == "title")
                return books.OrderBy(x => x.Title);
            return books.OrderBy(x => x.Author);
        }
        [HttpGet("{id:int}")]
        public async Task<IEnumerable<BookDetailDto>> GetBookById(int id)
        {
            var book = _context.Books.Include(x => x.ratings)
                        .Include(y => y.reviews).First(x => x.Id == id);
            var b = new List<BookDetailDto>();
            b.Add(new BookDetailDto()
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Cover = book.Cover,
                Content = book.Content,
                Rating = book.ratings.Average(x => x.Score),
                reviews = book.reviews
            });
            return b;
        }
    }
}
