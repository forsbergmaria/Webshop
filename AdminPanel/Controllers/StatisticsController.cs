using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
