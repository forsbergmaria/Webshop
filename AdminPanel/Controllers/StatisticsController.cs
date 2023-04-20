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
            var topFiveItems = _itemRepository.GetTopFiveMostSoldItems();
            var soldItemCounts = _itemRepository.GetSoldItemCountForEachItem();

            var model = new StatisticsViewModel
            {
                TopSoldItems = topFiveItems,
                SoldItemCountForEachItem = soldItemCounts
            };
            return View(model);
        }

        public IActionResult ShowTotalPurchases(string startDate, string endDate)
        {
           var sales = _itemRepository.GetTotalSalesSinceStart();
            return View(sales);
        }
    }
}
