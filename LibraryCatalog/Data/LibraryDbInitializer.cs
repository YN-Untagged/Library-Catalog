using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryCatalog.Models;

namespace LibraryCatalog.Data
{
    public class LibraryDbInitializer
    {
        public static void FineUpdate(LibraryDbContext _context)
        {
            var loans = _context.BookLoans.Where(r => r.ReturnByDate.Date < System.DateTime.Today.Date)
                .Where(r => r.ReturnByDate.Year != 0001).Where(r => r.ReturnDate.Year == 0001).Where(f => f.FineSettled == false);
            foreach (var loan in loans)
            {
                loan.Fine = true;
                loan.FineAmount = ((System.DateTime.Today.Date - loan.ReturnByDate.Date).Days) * 3;
            }
            _context.BookLoans.UpdateRange(loans);
            _context.SaveChanges();

        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {

            if(!roleManager.RoleExistsAsync("Member").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Member";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Librarian").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Librarian";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Administrator";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            string password = "PassWord123!";
            ApplicationUser member = new ApplicationUser()
            {
                Email = "Member@tmail.com",
                UserName = "Member@tmail.com",
                FirstName = "Member",
                LastName = "Keller",
                CreatedDate = System.DateTime.Today,
                EmailConfirmed = true,
                Status = 0,
                DateOfBirth = System.DateTime.Today.AddYears(-40),
                Gender = "Female"
            };
            userManager.CreateAsync(member, password);
            userManager.AddToRoleAsync(member, "Member");


            ApplicationUser librarian = new ApplicationUser()
            {
                Email = "Librarian@tmail.com",
                UserName = "Librarian@tmail.com",
                FirstName = "Librarian",
                LastName = "Keller",
                CreatedDate = System.DateTime.Today,
                EmailConfirmed = true,
                Status = 0,
                DateOfBirth = System.DateTime.Today.AddYears(-54),
                Gender = "Male"
            };
            userManager.CreateAsync(librarian, password);
            userManager.AddToRoleAsync(librarian, "Librarian");


            ApplicationUser admin = new ApplicationUser()
            {
                Email = "Admin@tmail.com",
                UserName = "Admin@tmail.com",
                FirstName = "Adminstrator",
                LastName = "Keller",
                CreatedDate = System.DateTime.Today,
                EmailConfirmed = true,
                Status = 0,
                DateOfBirth = System.DateTime.Today.AddYears(-30),
                Gender = "Female"
            };
            userManager.CreateAsync(admin, password);
            userManager.AddToRoleAsync(admin, "Administrator");
        }

        public static void Initialize(LibraryDbContext _context)
        {
            if (_context.Books.Any())
            {
                return;
            }

            var books = new List<Book>
            {
                new Book { ISBN = "9781612680194", Title = "Rich Dad Poor Dad", Year = 1997, Image = "rich-dad-poor-dad.jpg", Pages = 336, Genre = "Finances", Publisher = "Plata Publishing" , Price = 283},
                new Book { ISBN = "9781501124020", Title = "Principles", Year = 2017, Image = "principles.jpg", Pages = 592, Genre = "Finances", Publisher = "Simon and Schuster" , Price = 245},
                new Book { ISBN = "9780141365374", Title = "Charlie and the Chocolate Factory", Year = 1964, Image = "charlie-and-the-chocolate-factory.jpg", Pages = 208, Genre = "Fantasy", Publisher = "Scholastic Corporation" , Price = 184},
                new Book { ISBN = "9780440343691", Title = "I heard an owl call my name", Year = 1967, Image = "i-heard-an-owl-call-my-name.jpg", Pages = 159, Genre = "Fiction", Publisher = "Clarke, Irwin & Company" , Price = 178},
                new Book { ISBN = "9780679420392", Title = "Animal Farm", Year = 1945, Image = "animal-farm.jpg", Pages = 112, Genre = "Fiction", Publisher = "Harcourt, Brace and Company" , Price = 178},
                new Book { ISBN = "9780712670760", Title = "The Road Less Traveled and Beyond", Year = 1978, Image = "the-road-less-traveled-and-beyond.jpg", Pages = 381, Genre = "Philosophy", Publisher = "Simon and Schuster" , Price = 368},
                new Book { ISBN = "9780679735656", Title = "The Race Beat", Year = 2006, Image = "the-race-beat.jpg", Pages = 528, Genre = "History", Publisher = "Alfred A. Knopf", Price = 289 },
                new Book { ISBN = "9781611856446", Title = "The Great Secret: The Classified World War II Disaster that Launched the War on Cancer", Year = 2020, Image = "the-great-secret.jpg", Pages = 400, Genre = "History", Publisher = "W.W Norton and Company" , Price = 186},
                new Book { ISBN = "9780195174373", Title = "What Does It All Mean?: A Very Short Introduction to Philosophy", Year = 1987, Image = "what-does-it-all-mean.jpg", Genre = "Philosophy", Publisher = "Oxford University Press" , Price = 289},
                new Book { ISBN = "9781531831547", Title = "Uninvited: Living Loved When You Feel Less Than, Left Out, and Lonely", Year = 2016, Image = "uninvited.jpg", Pages = 288, Genre = "Religious", Publisher = "Thomas Nelson" , Price = 395},
                new Book { ISBN = "9780743264747", Title = "Einstein: His Life and Universe", Year = 2007, Image = "einstein.jpg", Pages = 675, Genre = "Biography", Publisher = "Simon and Schuster", Price = 259 },
                new Book { ISBN = "9781451688092", Title = "Team of Rivals: The Political Genius of Abraham Lincoln", Year = 2005, Image = "team-of-rivals.jpg", Pages = 944, Genre = "Biography", Publisher = "Simon and Schuster" , Price = 187},
                new Book { ISBN = "9780679443544", Title = "Helen Keller: A Determined Life	Elizabeth MacLeod", Year = 1998, Image = "a-determined-life.jpg", Pages = 32, Genre = "Biography", Publisher = "Alfred A. Knopf" , Price = 368},
                new Book { ISBN = "9780446969833", Title = "All the President's Men	Bob Woodward & Carl Bernstein", Year = 1974, Image = "all-the-presidents-men.jpg", Pages = 349, Genre = "Politics", Publisher = "Simon and Schuster" , Price = 300},
                new Book { ISBN = "9781776090891", Title = "The Republic of Gupta: A Story of State Capture	Pieter-Louis Myburgh", Year = 2017, Image = "republic-of-gupta.jpg", Pages = 1, Genre = "Politics", Publisher = "Penguin Books", Price = 287 }
            };
            books.ForEach(b => _context.Books.Add(b));
            _context.SaveChanges();

            var authors = new List<Author>
             {
                 new Author { FullName = "Robert Kiyosaki" },
                 new Author { FullName = "Ray Dalio" },
                 new Author { FullName = "Roald Dahl" },
                 new Author { FullName = "Marget Craven" },
                 new Author { FullName = "Goerge Orwell" },
                 new Author { FullName = "M.Scott Peck" },
                 new Author { FullName = "Gene Roberts" },
                 new Author { FullName = "Hank Klibanoff" },
                 new Author { FullName = "Jennet Conant" },
                 new Author { FullName = "Thomas Nagel" },
                 new Author { FullName = "Lysa TerKeurst" },
                 new Author { FullName = "Walter Isaacson" },
                 new Author { FullName = "Doris Kearns Goodwin" },
                 new Author { FullName = "Elizabeth MacLeod " },
                 new Author { FullName = "Bob Woodward" },
                 new Author { FullName = "Carl Bernstein" },
                 new Author { FullName = "Pieter-Louis Myburgh" }
             };
            authors.ForEach(a => _context.Authors.Add(a));
            _context.SaveChanges();

            var bookAuthors = new List<BookAuthor>
            {
               new BookAuthor { AuthorId = 1, BookId = 2 },
               new BookAuthor { AuthorId = 2, BookId = 3 },
               new BookAuthor { AuthorId = 3, BookId = 4 },
               new BookAuthor { AuthorId = 4, BookId = 5},
               new BookAuthor { AuthorId = 5, BookId = 6 },
               new BookAuthor { AuthorId = 6, BookId = 7 },
               new BookAuthor { AuthorId = 7, BookId = 8 },
               new BookAuthor { AuthorId = 8, BookId = 8 },
               new BookAuthor { AuthorId = 9, BookId = 9 },
               new BookAuthor { AuthorId = 10, BookId = 10 },
               new BookAuthor { AuthorId = 11, BookId = 11 },
               new BookAuthor { AuthorId = 12, BookId = 12 },
               new BookAuthor { AuthorId = 13, BookId = 13 },
               new BookAuthor { AuthorId = 14, BookId = 14 },
               new BookAuthor { AuthorId = 15, BookId = 15 },
               new BookAuthor { AuthorId = 16, BookId = 15 },
               new BookAuthor { AuthorId = 17, BookId = 16 }
            };
            bookAuthors.ForEach(ba => _context.BookAuthors.Add(ba));
            _context.SaveChanges();

        }

    }
}
