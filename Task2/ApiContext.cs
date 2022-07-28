using Microsoft.EntityFrameworkCore;
using Task2.Models;

namespace Task2
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Review> Reviews { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
