using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class OrdersController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
