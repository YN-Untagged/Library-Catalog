using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LibraryCatalog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibraryCatalog.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "First Name(s)")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            public string Gender { get; set; }

            
            [Display(Name = "Date of Birth")]
            public DateTime DateOfBirth { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            bool PhoneAdded = true;
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    PhoneAdded = false;
                    //return RedirectToPage();
                }
            }

            var upUser = user;
            {
                upUser.FirstName = Input.FirstName;
                upUser.LastName = Input.LastName;
                upUser.Gender = Input.Gender;
                upUser.DateOfBirth = Input.DateOfBirth;
                upUser.EditedBy = user.Id;
                upUser.EditedDate = System.DateTime.Now;
            }

            var updateDetails = await _userManager.UpdateAsync(user);
            if (!updateDetails.Succeeded || !PhoneAdded)
            {
                StatusMessage = "Unexpected error when trying to update details";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
