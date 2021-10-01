using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }

    }
}
