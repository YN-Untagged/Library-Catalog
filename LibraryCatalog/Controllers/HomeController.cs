using LibraryCatalog.Interfaces;
using LibraryCatalog.Models;
using LibraryCatalog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        
        public IActionResult Index()
        {
            if (User.IsInRole("Member"))
            {
                return RedirectToAction("DashBoard", "Member");

            }
            else if (User.IsInRole("Librarian") || User.IsInRole("Administrator"))
            {
                return RedirectToAction("DashBoard", "Librarian");
            }
            else
            {
                return RedirectToAction("Index", "Book");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
