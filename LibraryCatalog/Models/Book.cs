using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Models
{
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int BookId { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(13)]
        public string ISBN { get; set; }

        [Required]
        public string Title { get; set; }
        public int Pages { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public int Year { get; set; }

        [DefaultValue("placeholder.jpg")]
        public string Image { get; set; }

        [Required]
        public string Publisher { get; set; }

        [DefaultValue("English")]
        public string Language { get; set; }
        public decimal Price { get; set; }

        [DefaultValue("Available")]
        public string Status { get; set; }
        public string Description { get; set; } 
        
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
        public virtual ICollection<BookLoan> BookLoans { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
