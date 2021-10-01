using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Models
{
    public class BookLoan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookLoanId { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }
        public string IssuedBy { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ReturnByDate { get; set; }
        public string ReceivedBy { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool Fine { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal FineAmount { get; set; }
        public bool FineSettled { get; set; }

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
