using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using LibraryCatalog.Models;

namespace LibraryCatalog.Controllers
{
    [Route("api/[controller]")]
    public class SendEmailController : Controller
    {
        private readonly IConfiguration _configuration;
        public SendEmailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("SendNotification")]
        public async Task PostMessage(Reservation reserve)
        {
            string receiverName = $"{reserve.ApplicationUser.FirstName} {reserve.ApplicationUser.LastName}";
            var apiKey = _configuration.GetSection("SENDGRID_API_KEY").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("LibraryCatalog@test.com", "Library Catalog");
            List<EmailAddress> tos = new List<EmailAddress>
          {
              new EmailAddress(reserve.ApplicationUser.Email, receiverName)
          };

            var subject = "Reserved book is now Available!";
            var plainTextContent = "";
            var link = "https://localhost:44376/Identity/Account/Login?ReturnUrl=%2FMember%2FDashboard";
            var linkStyle = "color:forestgreen; font:italic 14px;";
            var htmlContent = $"<strong>Hello {receiverName}! <br> The book you have reserved :{reserve.Book.Title} is now available. Hurry and get from the library <br> <a href={link} style={linkStyle}>Click here  to go libraby>></a></strong>";
            var displayRecipients = false; 
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, plainTextContent, htmlContent, displayRecipients);
            var response = await client.SendEmailAsync(msg);

        }
    }
}
