using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Models;
using Data;

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

            var transactionList = new List<StockTransaction>();
            var transactionListSizes = new List<StockTransactionSizes>();

            foreach (var item in items)
            {
                if(item.HasSize == true)
                {
                    transactionListSizes = _dbContext.StockTransactionSizes.Where(t => t.TransactionType == "Försäljning"
                    && t.TransactionDate.Date == DateTime.Today.Date).ToList();
                } 
                else
                {
                    transactionList = _dbContext.StockTransactions.Where(t => t.TransactionType == "Försäljning"
                    && t.TransactionDate.Date == DateTime.Today.Date).ToList();


                }

            }

            //foreach (var item in items)
            //{

            //}

            int totalSoldUnits = transactionList.Sum(t => t.Quantity) + transactionListSizes.Sum(t => t.Quantity);

            var itemsSold = new List<String>();

            var query = from t in transactionList
                        join i in items on t.ItemId equals i.ItemId orderby t.Quantity descending
                        join ts in transactionListSizes on i.ItemId equals ts.ItemId orderby ts.Quantity descending
                        select new List<string>
                        {
                            i.Name
                        };

                        var model = new HomeViewModel
                        {
                            UserFirstName = currentUser.FirstName,
                            UnitsSold = totalSoldUnits,
                            ItemsSold = (List<string>)query.Take(5)
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