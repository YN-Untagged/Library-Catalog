using LibraryCatalog.Data;
using LibraryCatalog.Interfaces;
using LibraryCatalog.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Repositories
{
    public class ReservationRepository : IReservation
    {
        private readonly LibraryDbContext _context;
        public ReservationRepository(LibraryDbContext context)
        {
            _context = context;
        }
        public void Add(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
        }

        public bool BookLoanExists(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Edit(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
        }

        public IQueryable<Reservation> GetReserved()
        {
            return _context.Reservations.Include(u => u.ApplicationUser).Include(b => b.Book);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
