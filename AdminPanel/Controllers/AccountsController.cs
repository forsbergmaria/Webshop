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
        private readonly UserManager<Admin> _userManager;
        public AccountsController(ApplicationDbContext dbContext, UserManager<Admin> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> AllAccounts()
        {
            var currentUser = await _userManager.GetUserAsync(User);


            List<Admin> users = _dbContext.Admins.ToList();
            List<IdentityRole> identityRoles = _dbContext.IdentityRoles.ToList();
            List<IdentityUserRole<string>> userRoles = _dbContext.UserRoles.ToList();

            var query = from u in users
                        join ur in userRoles on u.Id equals ur.UserId
                        join i in identityRoles on ur.RoleId equals i.Id
                        select new AdminViewModel
                        {
                            Id = u.Id,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Email = u.Email,
                            Role = i.Name
                        };

            var identityRole = _dbContext.UserRoles.Where(u => u.UserId == currentUser.Id).FirstOrDefault();
            var userRole = _dbContext.IdentityRoles.Where(i => i.Id == identityRole.RoleId).FirstOrDefault();

            if (userRole.Name == "Huvudadministratör")
            {
                ViewBag.Role = "Huvudadministratör";
            }
            else
            {
                ViewBag.Role = "Moderator";
            }


            return View(query.ToList());
        }

        [Authorize]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize]
        public IActionResult ManageAccount() 
        {
            return View();
        }

        [Authorize]
        public IActionResult DeleteAccount(string id)
        {
            var userRole = _dbContext.UserRoles.Where(u => u.UserId.Equals(id)).FirstOrDefault();
            _dbContext.Remove(userRole);
            _dbContext.SaveChanges();

            var user = _dbContext.Admins.Where(u => u.Id.Equals(id)).FirstOrDefault();
            _dbContext.Remove(user);
            _dbContext.SaveChanges();

            return RedirectToAction("Home", "Home");
        }
    }
}
