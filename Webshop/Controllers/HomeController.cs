using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using DataAccess;

namespace Webshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext dbContext;
        private HttpClient _httpClient; // skapa http client
        public HomeController(ILogger<HomeController> logger, HttpClient httpClient, ApplicationDbContext dbContext)
        {
            _logger = logger;
            this._httpClient = httpClient;
            this.dbContext = dbContext;
        }


        public async Task<IActionResult> Items() // async därför vi hämtar data från server
        {

            List<Item> allItems = new();
            allItems = await GetAllItemsAsync();
            var theItems = allItems.Where(x => x.IsPublished == true).ToList();

            return View(theItems);

        }

        public async Task<List<Item>> GetAllItemsAsync() // hämta alla produkter
        {
            HttpResponseMessage response = await _httpClient.GetAsync("Item");
            string data = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var itemList = JsonSerializer.Deserialize<List<Item>>(data, options);
            return itemList;
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}