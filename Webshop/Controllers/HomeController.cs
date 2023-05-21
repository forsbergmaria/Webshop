using Microsoft.AspNetCore.Mvc;
using Models;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using Data;
using DataAccess.Data.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccess.Data.Repositories;
using Models.ViewModels;

namespace Webshop.Controllers
{
    public class HomeController : Controller
    {
        ItemRepository itemRepository { get { return new ItemRepository(); } }
        IHttpContextAccessor _ca => new HttpContextAccessor();
        ShoppingCartManager _cm { get { return new ShoppingCartManager(_ca); } }
        StatisticsRepository statisticsRepository { get { return new StatisticsRepository(); } }
        ItemService itemService { get { return new ItemService(); } }

        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            List<Item> mostPopularItems = statisticsRepository.GetMostSoldItems(10, DateTime.Now.AddDays(-14), DateTime.Now);
            List<HomePageViewModel> model = new List<HomePageViewModel>();

            foreach(Item item in mostPopularItems)
            {
                var m = new HomePageViewModel {
                    Item = item,
                    Image = itemRepository.GetImagesByItemId(item.ItemId).FirstOrDefault()
                };

                model.Add(m);
            }

            var cartItems = _cm.GetCartItems();
            ViewBag.Quantity = cartItems.Quantity;

            return View(model);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Items()
        {
            //var items = itemService.SearchByCategory(searchString);
            var categorylist = itemService.PopulateCategoryList();
            ViewBag.CategoryList = categorylist;
            return View(categorylist);
        }
    }
}