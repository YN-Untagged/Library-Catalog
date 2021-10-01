using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId { get; set; }

        [Required]
        [Display(Name = "Name(s)")]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get => Name + " " + Surname; }

        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
