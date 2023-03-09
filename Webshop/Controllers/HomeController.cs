//using Microsoft.AspNetCore.Mvc;
//using Models;
//using System.Diagnostics;
//using System.Net.Http;
//using System.Text.Json;
//using Microsoft.AspNetCore.Authorization;
//using System.Text.Json.Serialization;
//using Microsoft.EntityFrameworkCore;
//using DataAccess;

//namespace Webshop.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;
//        private ApplicationDbContext dbContext;
//        private HttpClient _httpClient; // skapa http client
//        public HomeController(ILogger<HomeController> logger, HttpClient httpClient, ApplicationDbContext dbContext)
//        {
//            _logger = logger;
//            this._httpClient = httpClient;
//            this.dbContext = dbContext;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        public async Task<IActionResult> Items() // async därför vi hämtar data från server
//        {

//            List<Item> allItems = new();
//            allItems = await GetAllItemsAsync();
//            var myMessages = allMessages.Where(x => x.ToUserID.Equals(user.Id)).ToList();
//            var items = allItems.Where(i => i.);

//            foreach (var item in items)
//            {
//                User fromuser = context.Users.Where(x => x.Id.Equals(item.FromUserID)).FirstOrDefault();
//                item.FromUser = fromuser;
//            }

//            return View(myMessages);

//        }

//        public async Task<List<Item>> GetAllItemsAsync() // hämta alla produkter
//        {
//            HttpResponseMessage response = await _httpClient.GetAsync("Message");
//            string data = await response.Content.ReadAsStringAsync();
//            var options = new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true,
//            };
//            var msgList = JsonSerializer.Deserialize<List<SentMessage>>(data, options);
//            return msgList;
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }
//    }
//}