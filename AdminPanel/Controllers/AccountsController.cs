using DataAccess;
using DataAccess.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Models;
using Data;

namespace AdminPanel.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountsController : Controller
    {
        UserRepository _userRepository { get { return new UserRepository(_userManager); } }


        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Admin> _userManager;
        public AccountsController(ApplicationDbContext dbContext, UserManager<Admin> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Huvudadministratör, Moderator")]
        public async Task<IActionResult> AllAccounts()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var users = _userRepository.GetAllUsers();
            var userRoles = _userRepository.GetAllIdentityRoles();
            var identityRoles = _userRepository.GetAllIdentityUserRoles();

            var query = from u in users
                        join ur in identityRoles on u.Id equals ur.UserId
                        join i in userRoles on ur.RoleId equals i.Id
                        select new AdminViewModel
                        {
                            Id = u.Id,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Email = u.Email,
                            Role = i.Name
                        };

            return View(query.ToList());
        }

        [Authorize(Roles = "Huvudadministratör")]
        public IActionResult DeleteAccount(string id)
        {
            _userRepository.DeleteAccount(id);
            return RedirectToAction("AllAccounts");
        }

        [Authorize(Roles = "Huvudadministratör")]
        public IActionResult Register()
        {
            var selectRole = _userRepository.GetAllIdentityRoles();
            var selectList = new SelectList(selectRole, "Name").OrderByDescending(r => r.Text);
            ViewBag.RolesList = selectList;

            return View("RegisterAccount");
        }


        [Authorize(Roles = "Huvudadministratör")]
        public async Task<IActionResult> RegisterAccount(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                Admin user = new Admin();

                user.FirstName = registerViewModel.FirstName;
                user.LastName = registerViewModel.LastName;
                user.UserName = registerViewModel.Email;
                user.Email = registerViewModel.Email;
                user.EmailConfirmed = true;
                var roleName = registerViewModel.Role;

                var result = await _userManager.CreateAsync(user);
                var role = _userRepository.GetIdentityRoleByName(roleName);

                if (result.Succeeded)
                {
                    _userRepository.AssignRole(user.Id, role.Id);
                    _dbContext.SaveChanges();

                    return RedirectToAction("AllAccounts", "Accounts");
                }
            }
            return View(registerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> SearchAdmin(string searchString)
        {
            var query = _dbContext.Admins.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                var searchTerms = searchString.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var term in searchTerms)
                {
                    query = query.Where(a => a.FirstName.Contains(term) || a.LastName.Contains(term));
                }
            }

            var admins = await query.ToListAsync();

            List<IdentityRole> identityRoles = _dbContext.Roles.ToList();
            List<IdentityUserRole<string>> userRoles = _dbContext.UserRoles.ToList();

            var query2 = from u in admins
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
            var currentUser = await _userManager.GetUserAsync(User);
            var identityRole = _dbContext.UserRoles.Where(u => u.UserId == currentUser.Id).FirstOrDefault();
            var userRole = _dbContext.Roles.Where(i => i.Id == identityRole.RoleId).FirstOrDefault();


            ViewBag.SearchString = searchString;

            return View(query2.ToList());
        }

        [Authorize(Roles = "Huvudadministratör")]
        public IActionResult UpdateAccountForm(string id)
        {
            var user = _dbContext.Admins.Where(a => a.Id == id).FirstOrDefault();
            var identityRole = _dbContext.UserRoles.Where(u => u.UserId == id).FirstOrDefault();
            var userRole = _dbContext.Roles.Where(i => i.Id == identityRole.RoleId).FirstOrDefault();

            var admin = new AdminViewModel
            {
                Id = id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = userRole.Name
            };

            var selectRole = _dbContext.Roles.ToList();
            var selectList = new SelectList(selectRole, "Name").OrderByDescending(r => r.Text);
            ViewBag.RolesList = selectList;

            TempData["id"] = id;
            return View("UpdateAccount", admin);
        }

        [Authorize(Roles = "Huvudadministratör")]
        public async Task<IActionResult> UpdateAccount(AdminViewModel model, string id)
        {
            var currentUser = await _userManager.FindByIdAsync(TempData["id"].ToString());

            currentUser.Email = model.Email;
            currentUser.FirstName = model.FirstName;
            currentUser.LastName = model.LastName;
            currentUser.UserName = model.Email;

            var userRole = _userRepository.GetIdentityRoleNameForUser(currentUser.Id);
            await _userManager.UpdateAsync(currentUser);
            if (userRole != model.Role)
            {
                await _userManager.RemoveFromRoleAsync(currentUser, userRole);
                await _userManager.AddToRoleAsync(currentUser, model.Role);
            }
            
            

            return RedirectToAction("AllAccounts");
        }
    }
}