using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public ValuesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public List<Item> GetAllPublishedItems()
        {
            return dbContext.Items.Where(i => i.IsPublished == true).ToList();
        }

        [HttpPut]
        public void PublishedAllItems() 
        { 
            List<Item> notPublished = dbContext.Items.ToList();
            foreach(var item in notPublished)
            {
                item.IsPublished = true;
            }

        }

        [HttpPut("{id}")]
        public void PublishItem([FromBody]Item item)
        {
            item.IsPublished = true;
            dbContext.Update(item);
            dbContext.SaveChanges();
        }

        [HttpPut("{id}")]
        public void UnpublishItem([FromBody]Item item)
        {
            item.IsPublished = false;
            dbContext.Update(item);
            dbContext.SaveChanges();
        }

    }
}
