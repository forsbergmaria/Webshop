using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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

        public Admin GetUserById(string id) 
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Admins.Where(u => u.Id == id).FirstOrDefault();
            }
        }

        public IdentityRole GetIdentityRole(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.IdentityRoles.Where(i => i.Id == id).FirstOrDefault();
            }
        }

        public string GetIdentityRoleName(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                var identityRole = GetIdentityRole(id);

                return identityRole.Name;
            }
        }

       public IdentityUserRole<string> GetIdentityUserRole(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.UserRoles.Where(u => u.UserId == id).FirstOrDefault();
            }
        }

        public List<IdentityRole> GetAllIdentityRoles()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.IdentityRoles.ToList();
            }
        }

        public List<IdentityUserRole<string>> GetAllIdentityUserRoles()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.UserRoles.ToList();
            }
        }

        [HttpDelete]
        public void DeleteAccount(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                var userRole = GetIdentityUserRole(id);
                context.Remove(userRole);
                context.SaveChanges();

                var user = GetUserById(id);
                context.Remove(user);
                context.SaveChanges();
            }
        }

    }
}
