using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public ValuesController(ApplicationDbContext dbContext) => _dbContext = dbContext;

        [HttpGet]
        public async Task<List<Item>> GetAllPublishedItems()
        {
            return await _dbContext.Items.ToListAsync();
        }
    }
}
