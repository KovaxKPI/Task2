using Task2.Models;

namespace Task2.Interfaces
{
    public interface IRecommendedService
    {
        IQueryable<BookDto> Get(string genre);
    }
}
