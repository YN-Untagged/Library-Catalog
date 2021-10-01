using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        
        public string CreatedBy { get; set; }

        [DefaultValue("getutcdate()")]
        public DateTime CreatedDate { get; set; }
        public string EditedBy { get; set; }

        public DateTime EditedDate { get; set; }

        [DefaultValue(UserStatus.Active)]
        public UserStatus Status { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Reservation> Reservation { get; set; }
        public virtual ICollection<BookLoan> BookLoans { get; set; }
    }
}