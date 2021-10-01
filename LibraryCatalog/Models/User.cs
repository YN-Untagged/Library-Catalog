using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string ContactNo { get; set; }
        public string UserType { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
