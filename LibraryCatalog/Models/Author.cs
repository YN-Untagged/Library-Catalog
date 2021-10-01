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
        public string FullName { get; set; }
        public string Biography { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string EditedBy { get; set; }
        public  DateTime EditedDate { get; set; }

        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
    }
}
