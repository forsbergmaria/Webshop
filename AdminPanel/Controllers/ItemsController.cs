using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class ItemsController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
