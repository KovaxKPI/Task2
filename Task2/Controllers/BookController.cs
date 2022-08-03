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
        private string secret = "qwerty";
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
                            Rating = b.ratings == null || b.ratings.Count == 0 ? 0 : b.ratings.Average(x => x.Score),
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
                            Rating = b.ratings.Count == 0 || b.ratings == null ? 0 : b.ratings.Average(x => x.Score),
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
                Rating = book.ratings == null || book.ratings.Count == 0 ?  0 : book.ratings.Average(x => x.Score),
                reviews = book.reviews
            });
            return b;
        }
        [HttpPost]
        public async Task<PostResponse> AddBook(BookPostDto book)
        {
            var b = new Book();
            b.Id = _context.Books.Count() + 1;
            b.Title = book.Title;
            b.Author = book.Author;
            b.Cover = book.Cover;
            b.Content = book.Content;
            b.Genre = book.Genre;
            b.reviews = new List<Review>();
            b.ratings = book.ratings;
            _context.Books.Add(b);
            await _context.SaveChangesAsync();
            return new PostResponse() { Id = b.Id};
        }
        [HttpDelete("{id:int}/{secret:alpha}")]
        public void DeleteBook(int id, string secret)
        {
            if(secret == this.secret)
            {
                _context.Books.Remove(_context.Books.First(x => x.Id == id));
                _context.SaveChangesAsync();
            }
        }
        [HttpPut("{id:int}/review")]
        public async Task<PostResponse> AddReview(int id, ReviewDto review)
        {
            Review r = new Review();
            r.Id = review.Id;
            r.Message = review.Message;
            r.Reviewer = review.Reviewer;
            r.BookId = id;
            _context.Reviews.Add(r);
            await _context.SaveChangesAsync();
            return new PostResponse() { Id = r.Id };
        }
        [HttpPut("{id:int}/rate")]
        public async Task<PostResponse> AddRate(int id, RatingDto rating)
        {
            Rating r = new Rating();
            r.Id = rating.Id;
            r.Score = rating.Score;
            r.BookId = id;
            _context.Ratings.Add(r);
            await _context.SaveChangesAsync();
            return new PostResponse() { Id = r.Id };
        }
    }
}
