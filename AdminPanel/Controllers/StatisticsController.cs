using Data;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AdminPanel.Controllers
{
    public class StatisticsController : Controller
    {
        ItemRepository _itemRepository { get { return new ItemRepository(); } }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowTotalPurchases(string startDate, string endDate)
        {
            return View();
        }
    }
}
