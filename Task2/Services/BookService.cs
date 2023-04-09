using Microsoft.EntityFrameworkCore;
using Task2.Interfaces;
using Task2.Models;

namespace Task2.Services
{
    public class BookService : IBookService
    {
        private readonly ApiContext _context;
        public BookService(ApiContext apiContext) 
        { 
            _context = apiContext; 
        }
        public async Task AddBook(BookPostDto book)
        {
            _context.Books.Add(new Book()
            {
                Id = _context.Books.Count() + 1,
                Title = book.Title,
                Author = book.Author,
                Cover = book.Cover,
                Content = book.Content,
                Genre = book.Genre,
                Reviews = new List<Review>(),
                Ratings = book.Ratings
            });
            await _context.SaveChangesAsync();
        }

        public async Task AddRate(int id, RatingDto rating)
        {
            _context.Ratings.Add(new Rating()
            {
                Id = rating.Id,
                BookId = id,
                Score = rating.Score,
            });
            await _context.SaveChangesAsync();
        }

        public async Task AddReview(int id, ReviewDto review)
        {
            _context.Reviews.Add(new Review() 
            { 
                Id = review.Id,
                BookId = id,
                Message= review.Message,
                Reviewer = review.Reviewer,
            });
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBook(int id)
        {
            var book = _context.Books.FirstOrDefault(x => x.Id == id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public BookDetailDto? GetBookById(int id)
        {
            var book = _context.Books.Include(x => x.Ratings)
                        .Include(y => y.Reviews).FirstOrDefault(x => x.Id == id);
            if (book == null)
                return null;
            return new BookDetailDto()
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Cover = book.Cover,
                Content = book.Content,
                Rating = book.Ratings == null || book.Ratings.Count == 0 ? 0 : book.Ratings.Average(x => x.Score),
                Reviews = book.Reviews
            };
        }

        public IQueryable<BookDto> GetBooks()
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

        public IQueryable<BookDto> GetBooksByOrder(string order)
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
    }
}
