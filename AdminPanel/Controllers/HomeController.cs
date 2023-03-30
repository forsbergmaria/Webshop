using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Models;
using Data;
using System.Linq;

namespace AdminPanel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Home()
        {
            var currentUser = _dbContext.Admins.Where(a => a.UserName.Equals(User.Identity.Name)).FirstOrDefault();
            var items = _dbContext.Items.ToList();

            var transactionList = new List<ItemTransaction>();

            foreach (var item in items)
            {

                {
                    transactionList = _dbContext.ItemTransactions.Where(t => t.TransactionType == "Försäljning"
                    && t.TransactionDate.Date == DateTime.Today.Date).ToList();
                }
            }

            int totalSoldUnits = transactionList.Sum(t => t.Quantity);


            var model = new HomeViewModel
            {
                UserFirstName = currentUser.FirstName,
                UnitsSold = totalSoldUnits
            };


            return View(model);     
        }
    
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}