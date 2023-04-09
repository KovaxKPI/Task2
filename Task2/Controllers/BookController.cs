using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task2.Interfaces;
using Task2.Models;

namespace Task2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService= bookService;
        }
        [HttpGet]
        public IQueryable<BookDto> GetBooks()
        {
            return _bookService.GetBooks();
        }
        [HttpGet("{order:alpha}")]
        public IQueryable<BookDto> GetBooksByOrder(string order)
        {
            return _bookService.GetBooksByOrder(order);
        }
        [HttpGet("{id:int}")]
        public BookDetailDto? GetBookById(int id)
        {
            return _bookService.GetBookById(id);
        }
        [HttpPost]
        public async Task AddBook(BookPostDto book)
        {
            await _bookService.AddBook(book);
        }
        [HttpDelete("delete/{id:int}")]
        public async Task DeleteBook(int id)
        {
            await _bookService.DeleteBook(id);
        }
        [HttpPut("{id:int}/review")]
        public async Task AddReview(int id, ReviewDto review)
        {
            await _bookService.AddReview(id, review);
        }
        [HttpPut("{id:int}/rate")]
        public async Task AddRate(int id, RatingDto rating)
        {
            await _bookService.AddRate(id, rating);
        }
    }
}
