using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
