using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Data
{
    public class AutoUpdates
    {
        private readonly LibraryDbContext _context;
        public AutoUpdates(LibraryDbContext context)
        {
            _context = context;
        }

        public static void UpdateFine(LibraryDbContext _context)
        {
            var loans = _context.BookLoans.Where(r => r.ReturnByDate.Date < System.DateTime.Today.Date)
                .Where(r => r.ReturnByDate.Year != 0001).Where(r => r.ReturnDate.Year == 0001).Where(f => f.FineSettled == false);
            foreach (var loan in loans)
            {
                loan.Fine = true;
                loan.FineAmount = (System.DateTime.Today.Date - loan.ReturnByDate.Date).Days * 3;
            }
            _context.BookLoans.UpdateRange(loans);

        }

        
    }
}
