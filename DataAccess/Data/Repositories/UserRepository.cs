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
        private readonly UserManager<Admin> _userManager;

        public UserRepository(UserManager<Admin> userManager)
        {
            _userManager = userManager;
        }
        // Returns a list of all the users from the database
        public List<Admin> GetAllUsers()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Admins.ToList();
            }
        }

        // Returns a specific user from the database 
        public Admin GetUserById(string id) 
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Admins.Where(u => u.Id == id).FirstOrDefault();
            }
        }

        // Returns a specific instance of the IdentityRole class
        public IdentityRole GetIdentityRoleByName(string roleName)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.IdentityRoles.Where(r => r.Name == roleName).FirstOrDefault();
            } 
        }

        // Returns an IdentityRole for a specific user
        public IdentityRole GetIdentityRoleForUser(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.IdentityRoles.Where(i => i.Id == id).FirstOrDefault();
            }
        }

        // Returns the name of an IdentityRole for a specific user
        public string GetIdentityRoleNameForUser(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                var role = context.UserRoles.Where(r => r.UserId == id).FirstOrDefault();
                var identityRole = context.IdentityRoles.Where(r => r.Id == role.RoleId).FirstOrDefault();

                return identityRole.Name;
            }
        }

        // Returns the IdentityUserRole for a specific user
       public IdentityUserRole<string> GetIdentityUserRole(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.UserRoles.Where(u => u.UserId == id).FirstOrDefault();
            }
        }

        // Returns a list of all IdentityRoles from the database
        public List<IdentityRole> GetAllIdentityRoles()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.IdentityRoles.ToList();
            }
        }

        // Returns a list of all IdentityUserRoles from the database
        public List<IdentityUserRole<string>> GetAllIdentityUserRoles()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.UserRoles.ToList();
            }
        }

        // Assigns a role to a specific user
        [HttpPost]
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

        public void ModifyUserRole(IdentityUserRole<string> identityRole)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Entry(identityRole).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void ModifyUser(Admin user)
        {
            using (var context = new ApplicationDbContext())
            {
            _userManager.UpdateAsync(user);
            context.SaveChanges();
            }

           
        }

        // Deletes a specific user from the database
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
