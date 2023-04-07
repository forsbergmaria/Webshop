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

        public IdentityRole GetIdentityRoleByName(string roleName)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.IdentityRoles.Where(r => r.Name == roleName).FirstOrDefault();
            } 
        }

        public IdentityRole GetIdentityRoleForUser(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.IdentityRoles.Where(i => i.Id == id).FirstOrDefault();
            }
        }

        public string GetIdentityRoleNameForUser(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                var identityRole = GetIdentityRoleForUser(id);

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

        public void AssignRole(string userId, string roleId)
        {
            using (var context = new ApplicationDbContext())
            {
                var identityRole = GetIdentityRoleForUser(userId);
                var assignedRole = new IdentityUserRole<string>
                {
                    RoleId = roleId,
                    UserId = userId
                };

                context.UserRoles.Add(assignedRole);
                context.SaveChanges();
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
