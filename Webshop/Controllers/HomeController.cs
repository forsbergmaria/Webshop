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

namespace Webshop.Controllers
{
    public class HomeController : Controller
    {
        ItemRepository itemRepository { get { return new ItemRepository(); } }
        ItemService itemService { get { return new ItemService(); } }

        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {

            return View();

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