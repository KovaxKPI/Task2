using Microsoft.AspNetCore.Mvc;
using Task2.Models;

namespace Task2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private ApiContext _context;
        public RatingController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Rating> Get()
        {
            return _context.Ratings.ToArray();
        }
        
    }
}