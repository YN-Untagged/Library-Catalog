using LibraryCatalog.Models;
using LibraryCatalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Interfaces
{
    public interface IBook
    {
        List<Book> GetBooks(string genre = null);
        void Add(Book book);
        void Edit(Book book);
        void Delete(Book book);
        Book  GetBookById(int id);
        List<BookAuthorViewModel> GetAuthors();
        IEnumerable<BookAuthor> GetBookAuthors();
        void Save();
        void Dispose();
        bool BookExists(int id);
    }
}
