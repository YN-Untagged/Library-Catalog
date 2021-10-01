using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryCatalog.Data;
using LibraryCatalog.Models;
using LibraryCatalog.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace LibraryCatalog.Controllers
{
    public class LoanController : Controller
    {
        private readonly LibraryDbContext _context;
        private readonly IUser _user;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBook _book;
        private readonly ILoan _loan;
        private readonly IReservation _reservation;
        private readonly IConfiguration _configuration;

        public LoanController(LibraryDbContext context, IUser user, UserManager<ApplicationUser> userManager, IBook book, ILoan loan, IReservation reservation, IConfiguration configuration)
        {
            _context = context;
            _user = user;
            _userManager = userManager;
            _book = book;
            _loan = loan;
            _reservation = reservation;
            _configuration = configuration;
        }

        // GET: Loan
        [Authorize(Policy = "writepolicy")]
        public ActionResult Index()
        {
            return View(_loan.GetBookLoans());
        }

        [Authorize]
        public ActionResult Requests()
        {
            var loans = _loan.GetBookLoans().Where(l => l.IssuedBy == null);
            return View(loans.ToList());
        }

        [Authorize]
        public ActionResult Outstanding()
        {
            var loans = _loan.GetBookLoans().Where(l => l.IssuedBy != null).Where(r => r.ReturnDate.Year == 0001);
            return View(loans.ToList());
        }

        // GET: Loan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookLoan = await _context.BookLoans
                .Include(b => b.ApplicationUser)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(m => m.BookLoanId == id);
            if (bookLoan == null)
            {
                return NotFound();
            }

            return View(bookLoan);
        }

        // GET: Loan/Create
        [Authorize]
        public IActionResult Create(int? Id)
        
        {
            ViewData["UserId"] = _user.GetUsers("Member");
            ViewData["BookId"] = new SelectList(_book.GetBooks(), "BookId", "Title");
            if (Id != null)
            {
                ViewData["Book"] = _book.GetBookById(Convert.ToInt32(Id));
            }
            //get current user
            ViewData["creatorID"] = _userManager.GetUserId(User); 
            return View();
        }

        // POST: Loan/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int? id,[Bind("BookLoanId,BookId,UserId,IssuedBy,IssueDate,ReturnByDate,ReturnDate,Fine")] BookLoan bookLoan, string btnCreate)
        {
            if (ModelState.IsValid)
            {
                var userID = _userManager.GetUserId(User);
                var user = _user.GetUserById(userID);
                if(user.Status != UserStatus.Active)
                {
                    TempData["message"] = "You have been blocked from loaning books.Request not successful.";
                    return RedirectToAction("Index", "Home");
                }

                if(btnCreate == "Request")
                {
                    bookLoan.IssuedBy = null;
                    TempData["message"] = "Book successfully request.";
                }
                else if(btnCreate == "Issue")
                {
                    bookLoan.IssueDate = System.DateTime.Now;
                    bookLoan.ReturnByDate = bookLoan.IssueDate.AddDays(10);
                    TempData["message"] = "Book successfully loaned.";
                }
                else if(btnCreate == "ReserverRequest")
                {
                    bookLoan.IssuedBy = null;
                    var reservation = _reservation.GetReserved().FirstOrDefault(r => r.ReservationId == Convert.ToInt32(id));
                    bookLoan.UserId = reservation.UserId;
                    bookLoan.BookId = reservation.BookId;
                    _reservation.Delete(reservation);
                    TempData["message"] = "Book successfully request.";
                }
                _loan.Add(bookLoan);
                _loan.Save();

                

                if (User.IsInRole("Administrator") || User.IsInRole("Librarian"))
                {
                    return RedirectToAction(nameof(Index)); 
                }
                else
                {
                    return this.RedirectToAction("DashBoard", "Member");
                }
            }

            return View(bookLoan);
        }

        [Authorize]
        // GET: Loan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookLoan = await _context.BookLoans.FindAsync(id);
            if (bookLoan == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bookLoan.UserId);
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title", bookLoan.BookId);
            return View(bookLoan);
        }

        // POST: Loan/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookLoanId,BookId,UserId,IssuedBy,IssueDate,ReturnByDate,ReturnDate,Fine")] BookLoan bookLoan)
        {
            if (id != bookLoan.BookLoanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _loan.Edit(bookLoan);
                    _loan.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookLoanExists(bookLoan.BookLoanId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bookLoan.UserId);
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "Title", bookLoan.BookId);
            return View(bookLoan);
        }

        [Authorize(Policy = "writepolicy")]
        public async Task<IActionResult> Return(int id, string btnEdit)
        {
            var bookLoan = _loan.GetBookLoanById(id);
            bookLoan.ReturnDate = System.DateTime.Now;
            bookLoan.ReceivedBy = _userManager.GetUserId(User);
            if (bookLoan.ReturnByDate < bookLoan.ReturnDate)
            {
                bookLoan.Fine = true;
            }
            if(btnEdit == "Settle")
            {
                bookLoan.FineSettled = true;
            }

            _loan.Edit(bookLoan);
            _loan.Save();

            //Get bookId and change book status
            var book = _book.GetBookById(bookLoan.BookId);
            var reservations = _reservation.GetReserved().Where(b => b.BookId == bookLoan.BookId);
            if(reservations.Any())
            {
                Reservation firstReservation = reservations.FirstOrDefault(d => d.ReserveDate == reservations.Min(r => r.ReserveDate));
                if (firstReservation != null)
                {
                    firstReservation.BookReturned = true;
                    _reservation.Edit(firstReservation);
                    _reservation.Save();
                    book.Status = "Reserved";
                    _book.Edit(book);
                    _book.Save();
                    var result = new SendEmailController(_configuration).PostMessage(firstReservation);
                }
            }
            else
            {
                book.Status = "Available";
            }

            _book.Edit(book);
            _book.Save();

            TempData["message"] = "Book: " + bookLoan.Book.Title + " has been returned by " + bookLoan.ApplicationUser.FirstName + " " + bookLoan.ApplicationUser.LastName;
            return RedirectToAction("Details", new { id = id.ToString() });
        }

        [Authorize]
        public RedirectToActionResult Reserve(int id, string btnReserve)
        {
            var userID = _userManager.GetUserId(User);
            var user = _user.GetUserById(userID);
            if (user.Status != UserStatus.Active)
            {
                TempData["message"] = "You have been blocked from loaning books.Book reservation failed.";
                return RedirectToAction("Index", "Home");
            }

            if (btnReserve == "Reserve")
            {
                var loan = _loan.GetBookLoans().Where(l => l.BookId == id).Where(l => l.ReturnByDate.Year != 0001).Where(l => l.ReturnDate.Year == 0001).FirstOrDefault();
                var reserve = new Reservation();
                {
                reserve.UserId = _userManager.GetUserId(User);
                reserve.ReserveDate = System.DateTime.Now;
                reserve.BookId = id;
                reserve.ExpectedDate = loan.ReturnByDate;
                }
                _reservation.Add(reserve);
                _reservation.Save();
                Book book = _book.GetBookById(id);
                book.Status = "Reserved";
                _book.Edit(book);
                _book.Save();


             TempData["message"] = "Book successfully reserved.";
            }
            else if(btnReserve == "Cancel")
            {

                var reserved = _reservation.GetReserved().FirstOrDefault(r => r.ReservationId == id); //Get reservation to be cancelled
                int bookID = reserved.BookId; 
                bool available = reserved.BookReturned;
                _reservation.Delete(reserved);
                _reservation.Save();
                var book = _book.GetBookById(bookID);

                if (available == true)//Check if book was ready to be requested(returned)
                {
                    //Prepare to notify next user(reserver) that book is available (check for other pending request) 
                    var reservations = _reservation.GetReserved().Where(b => b.BookId == bookID);
                    if(reservations.Any())
                    {
                        DateTime minDate = reservations.Min(d => d.ReserveDate);
                        var nextReservation = reservations.FirstOrDefault(d => d.ReserveDate == minDate);
                        {
                            nextReservation.BookReturned = true;
                        }
                        _reservation.Edit(nextReservation);
                        _reservation.Save();
                    }
                    else
                    {
                        book.Status = "Available";
                        _book.Edit(book);
                        _book.Save();
                    }
                }
                TempData["message"] = "Book successfully reservation successfully cancelled.";
            }
            return RedirectToAction("DashBoard", "Member");
        }


        [Authorize(Policy = "writepolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Issue(int id, string issueButton)
        {
            var loan = _loan.GetBookLoanById(id);
            if (issueButton == "Issue")
            {

                if (loan == null)
                {
                    return NotFound();
                }
                var bookId = loan.BookId;
                var book = _book.GetBookById(bookId);
                if (book.Status == "Available" || book.Status == "Reserved")
                {
                    loan.IssuedBy = _userManager.GetUserId(User);
                    loan.IssueDate = System.DateTime.Now;
                    loan.ReturnByDate = System.DateTime.Now.AddDays(10);
                    _loan.Edit(loan);
                    _loan.Save();
                    book = _book.GetBookById(bookId);
                    book.Status = "Checked_Out";
                    _book.Edit(book);
                    _book.Save();
                    ViewData["message"] = "Book issued to user.";
                }
                TempData["message"] = "Book successfully issued to: " + loan.ApplicationUser.FirstName + " " + loan.ApplicationUser.LastName;
            }
            else if(issueButton == "Reject")
            {
                _loan.Delete(loan);
                _loan.Save();
                TempData["message"] = "Loan request successfully rejected.";
            }

            return RedirectToAction(nameof(Requests));
        }


    // GET: Loan/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookLoan = await _context.BookLoans
            .Include(b => b.ApplicationUser)
            .Include(b => b.Book)
            .FirstOrDefaultAsync(m => m.BookLoanId == id);
            if (bookLoan == null)
            {
                return NotFound();
            }

            return View(bookLoan);
        }

    // POST: Loan/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookLoan = await _context.BookLoans.FindAsync(id);
            var book = _book.GetBookById(bookLoan.BookId);
            book.Status = "Available";
            _loan.Delete(bookLoan);
            _loan.Save();
            _book.Edit(book);
            _book.Save();
            return RedirectToAction(nameof(Index));
            }

            private bool BookLoanExists(int id)
            {
                return _context.BookLoans.Any(e => e.BookLoanId == id);
            }
        }

}


