using Microsoft.EntityFrameworkCore;
using Task2.Models;

namespace Task2.EF
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApiContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApiContext>>()))
            {
                // Look for any board games.
                if (context.Books.Any())
                {
                    return;   // Data was already seeded
                }

                context.Books.AddRange(
                    new Book
                    {
                        Id = 1,
                        Title = "Candy Land",
                        Cover = "Hasbro",
                        Content = "ABC",
                        Author = "Sasha",
                        Genre = "Drama"
                    },
                    new Book
                    {
                        Id = 2,
                        Title = "Carcasonne",
                        Cover = "Buran",
                        Content = "Z-Man Games",
                        Author = "Alex",
                        Genre = "Thriller"
                    },
                    new Book
                    {
                        Id = 3,
                        Title = "The Settlers of Catan",
                        Cover = "Taran",
                        Content = "Catan Studio",
                        Author = "Max",
                        Genre = "Fantasy"
                    });
                context.SaveChanges();
                context.Ratings.AddRange(
                    new Rating
                    {
                        Id = 1,
                        BookId = 2,
                        Score = 5
                    },
                    new Rating
                    {
                        Id = 2,
                        BookId = 3,
                        Score = 7
                    },
                    new Rating
                    {
                        Id = 3,
                        BookId = 1,
                        Score = 10
                    });
                context.SaveChanges();
                context.Reviews.AddRange(
                    new Review
                    {
                        Id = 1,
                        Message = "Good",
                        BookId = 2,
                        Reviewer = "Ostap"
                    },
                    new Review
                    {
                        Id = 2,
                        BookId = 3,
                        Message = "Norm",
                        Reviewer = "Nick"
                    },
                    new Review
                    {
                        Id = 3,
                        BookId = 1,
                        Message = "Ok",
                        Reviewer = "Lia"
                    });
                context.SaveChanges();
            }
        }
    }
}
