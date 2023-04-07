using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data.Repositories
{
    public class UserRepository
    {
        public List<Admin> GetAllUsers()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Admins.ToList();
            }
        }

        public string GetUserRole(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                var users = GetAllUsers();
                List<IdentityRole> identityRoles = context.IdentityRoles.ToList();
                List<IdentityUserRole<string>> userRoles = context.UserRoles.ToList();
                var identityRole = context.UserRoles.Where(u => u.UserId == id).FirstOrDefault();
                var userRole = context.IdentityRoles.Where(i => i.Id == identityRole.RoleId).FirstOrDefault();

                return userRole.Name;
            }
        }

        public List<IdentityRole> GetAllRoles()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.IdentityRoles.ToList();
            }
        }

        public List<IdentityUserRole<string>> GetAllIdentityRoles()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.UserRoles.ToList();
            }
        }
    }
}
