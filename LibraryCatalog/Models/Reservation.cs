using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationId { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public DateTime ReserveDate { get; set; }
        public bool BookReturned { get; set; }
        public DateTime? ExpectedDate { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }
    }
}
