using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class ItemsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
