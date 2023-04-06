﻿using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Models;

namespace AdminPanel.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountsController : Controller
    {
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

            return View(query.ToList());
        }

        [Authorize(Roles = "Huvudadministratör")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpDelete]
        [Authorize(Roles = "Huvudadministratör")]
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

        [Authorize(Roles = "Huvudadministratör")]
        public IActionResult RegisterForm()
        {
            var selectRole = _dbContext.IdentityRoles.ToList();
            var selectList = new SelectList(selectRole, "Name").OrderByDescending(r => r.Text);
            ViewBag.RolesList = selectList;

            return View("RegisterAccount");
        }

        [HttpPost]
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

                var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                var role = _dbContext.IdentityRoles.Where(r => r.Name.Equals(roleName)).FirstOrDefault();

                if (result.Succeeded)
                {

                    var assignedRole = new IdentityUserRole<string>
                    {
                        RoleId = role.Id,
                        UserId = user.Id
                    };

                    _dbContext.UserRoles.Add(assignedRole);
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

            List<IdentityRole> identityRoles = _dbContext.IdentityRoles.ToList();
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
            var userRole = _dbContext.IdentityRoles.Where(i => i.Id == identityRole.RoleId).FirstOrDefault();


            ViewBag.SearchString = searchString;

            return View(query2.ToList());
        }

        [Authorize(Roles = "Huvudadministratör")]
        public IActionResult UpdateAccountForm(string id)
        {
            var user = _dbContext.Admins.Where(a => a.Id == id).FirstOrDefault();
            var identityRole = _dbContext.UserRoles.Where(u => u.UserId == id).FirstOrDefault();
            var userRole = _dbContext.IdentityRoles.Where(i => i.Id == identityRole.RoleId).FirstOrDefault();

            var admin = new AdminViewModel
            {
                Id = id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = userRole.Name
            };

            var selectRole = _dbContext.IdentityRoles.ToList();
            var selectList = new SelectList(selectRole, "Name").OrderByDescending(r => r.Text);
            ViewBag.RolesList = selectList;
            //var currentUser = await _userManager.GetUserAsync(User);
            //var identityRole = _dbContext.UserRoles.Where(u => u.UserId == currentUser.Id).FirstOrDefault();
            //var userRole = _dbContext.IdentityRoles.Where(i => i.Id == identityRole.RoleId).FirstOrDefault();
            //var model = new AdminViewModel
            //{
            //    Id = id,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    Email = user.Email,
            //    Role = user
            //};  
            TempData["id"] = id;
            return View("UpdateAccount", admin);
        }

        [Authorize(Roles = "Huvudadministratör")]
        public async Task<IActionResult> UpdateAccount(AdminViewModel model)
        {
            var admin = new AdminViewModel
            {
                Id = TempData["id"].ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Role = model.Role
            };

            if (ModelState.IsValid)
            {
                var user = new Admin();

            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.UserName = model.Email;
            var userRole = model.Role;

            var role = _dbContext.IdentityRoles.Where(r => r.Name == userRole).FirstOrDefault();

                


                    var assignedRole = new IdentityUserRole<string>
                    {
                        RoleId = role.Id,
                        UserId = user.Id
                    };

                    _dbContext.UserRoles.Update(assignedRole);
                    _dbContext.SaveChanges();
                }
                return RedirectToAction("AllAccounts");
        }
    }
}