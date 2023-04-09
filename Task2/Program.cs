using Microsoft.EntityFrameworkCore;
using Task2.EF;
using Task2.Interfaces;
using Task2.Services;

namespace Task2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase(databaseName : "Library"));
            builder.Services.AddTransient<IBookService, BookService>();
            builder.Services.AddTransient<IRecommendedService, RecommendedService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                //3. Get the instance of BoardGamesDBContext in our services layer
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApiContext>();

                //4. Call the DataGenerator to create sample data
                DataGenerator.Initialize(services);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}