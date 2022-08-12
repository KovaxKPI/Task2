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
        public IEnumerable<BookDto> GetBooks()
        {
            var books = from b in _context.Books.Include(x => x.Ratings)
                        .Include(y => y.Reviews)
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
        [HttpGet("{order:alpha}")]
        public IEnumerable<BookDto> GetBooksByOrder(string order)
        {
            var books = from b in _context.Books.Include(x => x.Ratings)
                        .Include(y => y.Reviews)
                        select new BookDto()
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            Rating = b.Ratings == null || b.Ratings.Count == 0 ? 0 : b.Ratings.Average(x => x.Score),
                            reviewsNumber = b.Reviews == null || b.Reviews.Count == 0 ? 0 : b.Reviews.Count()
                        };
            if (order == "title")
                return books.OrderBy(x => x.Title);
            return books.OrderBy(x => x.Author);
        }
        [HttpGet("{id:int}")]
        public IEnumerable<BookDetailDto> GetBookById(int id)
        {
            var book = _context.Books.Include(x => x.Ratings)
                        .Include(y => y.Reviews).First(x => x.Id == id);
            var b = new List<BookDetailDto>();
            b.Add(new BookDetailDto()
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Cover = book.Cover,
                Content = book.Content,
                Rating = book.Ratings == null || book.Ratings.Count == 0 ?  0 : book.Ratings.Average(x => x.Score),
                Reviews = book.Reviews
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
            b.Reviews = new List<Review>();
            b.Ratings = book.Ratings;
            _context.Books.Add(b);
            await _context.SaveChangesAsync();
            return new PostResponse() { Id = b.Id};
        }
        [HttpDelete("{id:int}")]
        public async void DeleteBook(int id)
        {
            var book = _context.Books.FirstOrDefault(x => x.Id == id);
            if(book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
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
