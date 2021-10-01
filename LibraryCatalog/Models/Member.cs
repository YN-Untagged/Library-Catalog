using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Models
{
    public class Member : User
    {
        public string MembershipStartDate { get; set; }
    }
}
