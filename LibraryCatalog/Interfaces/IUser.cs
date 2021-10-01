using LibraryCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Interfaces
{
    public interface IUser
    {
        List<ApplicationUser> GetUsers(string role);
        string GetUserId();
        void Add(ApplicationUser user);
        void Edit(ApplicationUser user);
        void Delete(ApplicationUser user);
        ApplicationUser GetUserById(string id);
        void Dispose();
        void Save();
        bool UserExists(string id);
    }
}
