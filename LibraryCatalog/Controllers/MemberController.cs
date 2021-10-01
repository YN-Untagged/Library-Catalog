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
using LibraryCatalog.Interfaces;
using LibraryCatalog.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace LibraryCatalog.Controllers
{
    public class MemberController : Controller
    {
        private readonly IUser _user;
        private readonly ILoan _loan;
        private readonly IBook _book;
        private readonly IReservation _reservation;
        private readonly UserManager<ApplicationUser> _userManager;

        public MemberController(IUser user, ILoan loan, IBook book, IReservation reservation, UserManager<ApplicationUser> userManager)
        {
           _user = user;
            _loan = loan;
            _book = book;
            _reservation = reservation;
            _userManager = userManager;
        }

        public static IEnumerable<SelectListItem> GetSelectLists<T>()
        {
            return (Enum.GetValues(typeof(T)).Cast<T>().Select(
                enumList => new SelectListItem() { Text = enumList.ToString(), Value = enumList.ToString() })).ToList();
        }

        // GET: Member
        [Authorize(Policy = "writepolicy")]
        public ActionResult Index()
        {
            return View(_user.GetUsers("Member"));
        }

        [Authorize]
        public ActionResult DashBoard()
        {
            string userId = _userManager.GetUserId(User);
            var reserved = _reservation.GetReserved().Where(u => u.UserId == _userManager.GetUserId(User));
            var loans = _loan.GetBookLoansById(userId);
            ViewData["userDetails"] = _user.GetUserById(userId);

            return View(new LoanViewModel
            {
                GetBookLoans = loans.Where(r=> r.ReturnDate.Year != 0001).ToList(),
                GetCheckedOut = loans.Where(d => d.ReturnByDate.Year != 0001).Where(d => d.ReturnByDate.Date > System.DateTime.Now).Where(r => r.ReturnDate.Year == 0001).ToList(),
                GetOverDued = loans.Where(d => d.ReturnByDate.Date < System.DateTime.Now).Where(r => r.ReturnDate.Year == 0001).Where(d => d.ReturnByDate.Year != 0001).ToList(),
                GetRequested = loans.Where(i => i.IssuedBy == null).ToList(),
                GetReserved = reserved.ToList()
            });
        }

        // GET: Member/Details/5
        [Authorize(Policy = "writepolicy")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _user.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Member/Create
        [Authorize(Policy = "writepolicy")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Member/Create
        [Authorize(Policy = "writepolicy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Email,FirstName,LastName,Gender,PhoneNumber,DateOfBirth,Status")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                _user.Add(user);
                _user.Dispose();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Member/Edit/5
        [Authorize(Policy = "writepolicy")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _user.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["Status"] = GetSelectLists<UserStatus>();
            return View(user);
        }

        // POST: Member/Edit
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
                    var updateUser = _user.GetUserById(id);
                    {
                        updateUser.EditedBy = _userManager.GetUserId(User);
                        updateUser.EditedDate = System.DateTime.Now;
                        updateUser.Status = user.Status;
                    }
                    
                    //await _userManager.UpdateAsync(updateUser);
                    _user.Edit(updateUser);
                    _user.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_user.UserExists(user.Id))
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

        // GET: Member/Delete/5
        [Authorize(Policy = "writepolicy")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _user.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Member/Delete/5
        [Authorize(Policy = "writepolicy")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = _user.GetUserById(id);
            _user.Delete(user);
            _user.Dispose();

            return RedirectToAction(nameof(Index));
        }
    }
}
