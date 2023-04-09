using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task2.Interfaces;
using Task2.Models;

namespace Task2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendedController : ControllerBase
    {
        private readonly IRecommendedService _recommendedService;
        public RecommendedController(IRecommendedService recommendedService)
        {
            _recommendedService = recommendedService;
        }

        [HttpGet]
        public IQueryable<BookDto> Get(string genre)
        {
            return _recommendedService.Get(genre);
        }
    }
}
