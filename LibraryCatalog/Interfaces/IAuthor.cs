using LibraryCatalog.Models;
using LibraryCatalog.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.Interfaces
{
    public interface IAuthor
    {
        IEnumerable<Author> GetAuthors();
        void Add(Author author);
        void Edit(Author author);
        void Delete(Author author);
        Author GetAuthorById(int id);
        List<BookAuthorViewModel> GetBooks();
        void AddBookAuthor(BookAuthor bookAuthor);
        void Save();
        void Dispose();
        bool AuthorExists(int id);
    }
}
