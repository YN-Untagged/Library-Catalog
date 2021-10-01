using LibraryCatalog.Data;
using LibraryCatalog.Interfaces;
using LibraryCatalog.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Repositories
{
    public class UserRepository : IUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LibraryDbContext _context;

        public UserRepository(UserManager<ApplicationUser> userManager, LibraryDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<ApplicationUser> GetUsers(string role)
        {
            return (List<ApplicationUser>)_userManager.GetUsersInRoleAsync(role).Result;
        }

        
        public ApplicationUser GetUserById(string id)
        {
            return _userManager.FindByIdAsync(id).Result;
        }

        public void Add(ApplicationUser user)
        {
             _userManager.CreateAsync(user);
        }

        public void Edit(ApplicationUser user)
        {
            _userManager.UpdateAsync(user);
            Save();
        }

        public void Delete(ApplicationUser user)
        {
             _userManager.DeleteAsync(user);
        }

        public ApplicationUser GetUserByID(string id)
        {
            return _userManager.FindByIdAsync(id).Result;
        }

        public bool UserExists(string id)
        {
            bool IsFound = false;


            if (GetUserByID(id) != null)
            {
                IsFound = true;
            }

            return IsFound;
        }

        public void Dispose()
        {
            _userManager.Dispose();
        }
        public string GetUserId()
        {
            return " ";//_userManager.GetUserId(User);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
