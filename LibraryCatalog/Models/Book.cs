using System;
using System.Collections.Generic;
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
        [MaxLength(13)]
        public string ISBN { get; set; }

        [Required]
        public string Title { get; set; }
        public int Pages { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public int Year { get; set; }
        public string URL { get; set; }

        [Required]
        public string Publisher { get; set; }

        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
