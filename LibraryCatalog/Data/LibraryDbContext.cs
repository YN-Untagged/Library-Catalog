using LibraryCatalog.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryCatalog.Data
{
    public class LibraryDbContext : IdentityDbContext<ApplicationUser>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookLoan> BookLoans { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
