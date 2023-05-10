using Data;
using DataAccess;
using DataAccess.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.ViewModels;

namespace AdminPanel.Controllers
{
    public class StatisticsController : Controller
    {
        StatisticsRepository _statisticsRepository { get { return new StatisticsRepository(); } }
        OrderRepository _orderRepository { get { return new OrderRepository(); } }
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = new StatisticsViewModel
            {
                MostSoldItemCountForEachItem = _statisticsRepository.GetMostSoldItems(5, DateTime.Now.AddDays(-30), DateTime.Now),
                LeastSoldItemCountForEachItem = _statisticsRepository.GetMostSoldItems(5, DateTime.Now.AddDays(-30), DateTime.Now),
                NumberOfSales = _statisticsRepository.GetTotalSalesSinceStart(),
                ItemsNeverSold = _statisticsRepository.GetTopItemsThatHaveNeverBeenSold(10),
            };

            ViewBag.UnhandledOrders = _orderRepository.GetNumberOfUnhandledOrders();

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public async Task<IActionResult> SearchItems(string searchString)
        {
            var query = _context.Items.Include(c => c.Category).Include(i => i.ProductImages)
                .Include(s => s.Subcategory)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                var searchTerms = searchString.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var term in searchTerms)
                {
                    query = query.Where(a => a.Name.Contains(term));
                }
            }

            var items = await query.ToListAsync();

            ViewBag.SearchString = searchString;
            ViewBag.UnhandledOrders = _orderRepository.GetNumberOfUnhandledOrders();

            return View(items);
        }
        public IActionResult ViewAllNeverSoldItems() 
        {
            var items = _statisticsRepository.GetAllItemsNeverSold();
            ViewBag.UnhandledOrders = _orderRepository.GetNumberOfUnhandledOrders();
            return View("NeverSoldItems", items);
        }
    }
}
