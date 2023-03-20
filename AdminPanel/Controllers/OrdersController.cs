using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
