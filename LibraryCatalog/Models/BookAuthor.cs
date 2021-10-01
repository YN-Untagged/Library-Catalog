using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Models
{
    public class BookAuthor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookAuthorId {get; set;}

        [ForeignKey("AuthorId")]
        public int AuthorId { get; set; }

        [ForeignKey("BookId")]
        public int BookId { get; set; }

        public virtual Book Book { get; set; }
        public virtual Author Author { get; set; }
    }
}
