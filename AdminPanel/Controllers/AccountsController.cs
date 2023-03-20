using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
