using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryCatalog.Data;
using LibraryCatalog.Models;
using Microsoft.AspNetCore.Identity;
using LibraryCatalog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using LibraryCatalog.Interfaces;

namespace LibraryCatalog.Controllers
{
    public class LibrarianController : Controller
    {
        private readonly ILoan _loan;
        private readonly IBook _book;
        private readonly UserManager<ApplicationUser> _userManager;

        public LibrarianController(IUser user, ILoan loan, IBook book, UserManager<ApplicationUser> userManager)
        {
            _book = book;
            _loan = loan;
            _userManager = userManager;
        }

        public ViewResult List()
        {
            var librarians = _userManager.GetUsersInRoleAsync("Librarian").Result;
            var Admins = _userManager.GetUsersInRoleAsync("Administrator").Result;
            var members = _userManager.GetUsersInRoleAsync("Member").Result;
            var users = _userManager.Users;
            var books = _book.GetBooks();
            var loans = _loan.GetBookLoans();
            var bestBook = _book.GetBookById(_loan.MostLoanedBook());
            var checkedOut = loans.Where(i => i.IssueDate.Year != 0001).Where(r => r.ReturnDate.Year == 0001);
            var overDued = checkedOut.Where(r => r.ReturnByDate.Date < System.DateTime.Now.Date);

            return View(new DashBoardViewModel
            {
                GetUsers = users,
                GetAdmins = Admins,
                GetLibrarians = librarians,
                GetMembers = members,
                GetBooks = books,
                GetLoans = loans,
                AmountOwed = loans.Where(s => s.FineSettled == false).Sum(f => f.FineAmount),
                CheckedOut = checkedOut,
                OverDued = overDued,
                MonthBook = bestBook,
                AllFemales = users.Where(g => g.Gender == "Female").Count(),
                AllMales = users.Where(g => g.Gender == "Male").Count(),
                mFemales = members.Where(g => g.Gender == "Female").Count(),
                mMales = members.Where(g => g.Gender == "Male").Count(),
            }) ;
        }

        // GET: Librarian
        [Authorize(Policy = "writepolicy")]
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.GetUsersInRoleAsync("Librarian"));
        }

        [Authorize(Policy = "writepolicy")]
        public ActionResult DashBoard()
        {
            List();
            return View();
        }
        // GET: Librarian/Details/5
        [Authorize(Policy = "writepolicy")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Librarian/Create
        [Authorize(Policy = "writepolicy")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Librarian/Create
        [Authorize(Policy = "writepolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,FirstName,LastName,Gender,PhoneNumber,DateOfBirth,Status,PasswordHashed ")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                await _userManager.CreateAsync(user);
                //await _userManager.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Librarian/Edit/5
        [Authorize(Policy = "writepolicy")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Librarian/Edit/5
        [Authorize(Policy = "writepolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Email,FirstName,LastName,Gender,PhoneNumber,DateOfBirth,Status")] ApplicationUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _userManager.UpdateAsync(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Librarian/Delete/5
        [Authorize(Policy = "writepolicy")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Librarian/Delete/5
        [Authorize(Policy = "writepolicy")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            var user = _userManager.FindByIdAsync(id);
            
            if(user != null)
            {
                return true;
            }
             
            return false;
        }
    }
}
