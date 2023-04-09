using Task2.Models;

namespace Task2.Interfaces
{
    public interface IBookService
    {
        IQueryable<BookDto> GetBooks();
        IQueryable<BookDto> GetBooksByOrder(string order);
        BookDetailDto? GetBookById(int id);
        Task AddBook(BookPostDto book);
        Task DeleteBook(int id);
        Task AddReview(int id, ReviewDto review);
        Task AddRate(int id, RatingDto rating);
    }
}
