using Microsoft.AspNetCore.Mvc;

namespace Webshop.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
