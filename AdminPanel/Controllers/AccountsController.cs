using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
            List<Admin> users = _dbContext.Admins.ToList();
            List<IdentityRole> identityRoles = _dbContext.IdentityRoles.ToList();
            List<IdentityUserRole<string>> userRoles = _dbContext.UserRoles.ToList();

            var query = from u in users
                        join ur in userRoles on u.Id equals ur.UserId
                        join i in identityRoles on ur.RoleId equals i.Id
                        select new AdminViewModel
                        {
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Email = u.Email,
                            Role = i.Name
                        };

            return View(query.ToList());
        }

        [Authorize]
        public IActionResult Register()
        {
            return View();
        }
    }
}
