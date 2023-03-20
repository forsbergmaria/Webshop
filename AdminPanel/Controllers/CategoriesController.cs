using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class CategoriesController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
