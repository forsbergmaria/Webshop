using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AdminPanel.Controllers
{
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult AllAccounts()
        {
            //var allAccounts = _dbContext.Admins.ToList();
            //var model = new Admin
            //{
            //    Id = allAccounts[0].Id,
            //    UserName = allAccounts[0].UserName,
            //    FirstName = allAccounts[0].FirstName,
            //    LastName = allAccounts[0].LastName,
            //    Email = allAccounts[0].Email
            //};

            //foreach(var account in allAccounts)
            //{
            //    model.
            //}
            //return View(model);
            var allAccounts = _dbContext.Admins.ToList();
            return View(allAccounts);
        }

        [Authorize]
        public IActionResult Register()
        {
            return View();
        }
    }
}
