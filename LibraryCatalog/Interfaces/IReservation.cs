using LibraryCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Interfaces
{
    public interface IReservation
    {
        IQueryable<Reservation> GetReserved();
        void Add(Reservation reservation);
        void Delete(Reservation reservation);
        void Edit(Reservation reservation);
        bool BookLoanExists(int id);
        void Save();
        void Dispose();
    }
}
