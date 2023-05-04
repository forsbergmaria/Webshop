using Data;
using DataAccess.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.ViewModels;

namespace AdminPanel.Controllers
{
    public class StatisticsController : Controller
    {
        StatisticsRepository _statisticsRepository { get { return new StatisticsRepository(); } }
        public IActionResult Index()
        {
            var model = new StatisticsViewModel
            {
                TopMostSoldItems = _statisticsRepository.GetTopMostSoldItems(5, DateTime.Now.AddDays(-7), DateTime.Now),
                MostSoldItemCountForEachItem = _statisticsRepository.GetMostSoldItemCountForEachItem(),
                TopLeastSoldItems = _statisticsRepository.GetTopLeastSoldItems(5, DateTime.Now.AddDays(-7), DateTime.Now),
                LeastSoldItemCountForEachItem = _statisticsRepository.GetLeastSoldItemCountForEachItem(),
                NumberOfSales = _statisticsRepository.GetTotalSalesSinceStart()
            };

            return View(model);
        }

        public IActionResult ShowTotalPurchases(string startDate, string endDate)
        {
            return View();
        }
    }
}
