using LibraryCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryCatalog.DAL
{
    public class LibraryInitializer
    {
        public static void Initialize(LibraryDbContext _context)
        {
            if (_context.Authors.Any())
            {
                return;
            }

            var books = new List<Book>
            {
                new Book { ISBN = "9781612680194", Title = "Rich Dad Poor Dad", Year = 1997, URL = "rich-dad-poor-dad.jpg", Pages = 336, Genre = "Finances", Publisher = "Plata Publishing" },
                new Book { ISBN = "9781501124020", Title = "Principles", Year = 2017, URL = "principles.jpg", Pages = 592, Genre = "Finances", Publisher = "Simon and Schuster" },
                new Book { ISBN = "9780141365374", Title = "Charlie and the Chocolate Factory", Year = 1964, URL = "charlie-and-the-chocolate-factory.jpg", Pages = 208, Genre = "Fantasy", Publisher = "Scholastic Corporation" },
                new Book { ISBN = "9780440343691", Title = "I heard an owl call my name", Year = 1967, URL = "i-heard-an-owl-call-my-name.jpg", Pages = 159, Genre = "Fiction", Publisher = "Clarke, Irwin & Company" },
                new Book { ISBN = "9780679420392", Title = "Animal Farm", Year = 1945, URL = "animal-farm.jpg", Pages = 112, Genre = "Fiction", Publisher = "Harcourt, Brace and Company" },
                new Book { ISBN = "9780712670760", Title = "The Road Less Traveled and Beyond", Year = 1978, URL = "the-road-less-traveled-and-beyond.jpg", Pages = 381, Genre = "Philosophy", Publisher = "Simon and Schuster" },
                new Book { ISBN = "9780679735656", Title = "The Race Beat", Year = 2006, URL = "the-race-beat.jpg", Pages = 528, Genre = "History", Publisher = "Alfred A. Knopf" },
                new Book { ISBN = "9781611856446", Title = "The Great Secret: The Classified World War II Disaster that Launched the War on Cancer", Year = 2020, URL = "the-great-secret.jpg", Pages = 400, Genre = "History", Publisher = "W.W Norton and Company" },
                new Book { ISBN = "9780195174373", Title = "What Does It All Mean?: A Very Short Introduction to Philosophy", Year = 1987, URL = "what-does-it-all-mean.jpg", Genre = "Philosophy", Publisher = "Oxford University Press" },
                new Book { ISBN = "9781531831547", Title = "Uninvited: Living Loved When You Feel Less Than, Left Out, and Lonely", Year = 2016, URL = "uninvited.jpg", Pages = 288, Genre = "Religious", Publisher = "Thomas Nelson" },
                new Book { ISBN = "9780743264747", Title = "Einstein: His Life and Universe", Year = 2007, URL = "einstein.jpg", Pages = 675, Genre = "Biography", Publisher = "Simon and Schuster" },
                new Book { ISBN = "9781451688092", Title = "Team of Rivals: The Political Genius of Abraham Lincoln", Year = 2005, URL = "team-of-rivals.jpg", Pages = 944, Genre = "Biography", Publisher = "Simon and Schuster" },
                new Book { ISBN = "9780679443544", Title = "Helen Keller: A Determined Life	Elizabeth MacLeod", Year = 1998, URL = "a-determined-life.jpg", Pages = 32, Genre = "Biography", Publisher = "Alfred A. Knopf" },
                new Book { ISBN = "9780446969833", Title = "All the President's Men	Bob Woodward & Carl Bernstein", Year = 1974, URL = "all-the-presidents-men.jpg", Pages = 349, Genre = "Politics", Publisher = "Simon and Schuster" },
                new Book { ISBN = "9781776090891", Title = "The Republic of Gupta: A Story of State Capture	Pieter-Louis Myburgh", Year = 2017, URL = "republic-of-gupta.jpg", Pages = 1, Genre = "Politics", Publisher = "Penguin Books" }
            };
            books.ForEach(b => _context.Books.Add(b));
            _context.SaveChanges();

            var bookAuthors = new List<BookAuthor>
            {
               new BookAuthor { AuthorId = 1, BookId = 1 },
               new BookAuthor { AuthorId = 2, BookId = 2 },
               new BookAuthor { AuthorId = 3, BookId = 3 },
               new BookAuthor { AuthorId = 4, BookId = 4 },
               new BookAuthor { AuthorId = 5, BookId = 5 },
               new BookAuthor { AuthorId = 6, BookId = 6 },
               new BookAuthor { AuthorId = 7, BookId = 7 },
               new BookAuthor { AuthorId = 8, BookId = 7 },
               new BookAuthor { AuthorId = 9, BookId = 8 },
               new BookAuthor { AuthorId = 10, BookId = 9 },
               new BookAuthor { AuthorId = 11, BookId = 10 },
               new BookAuthor { AuthorId = 12, BookId = 11 },
               new BookAuthor { AuthorId = 13, BookId = 12 },
               new BookAuthor { AuthorId = 14, BookId = 13 },
               new BookAuthor { AuthorId = 15, BookId = 14 },
               new BookAuthor { AuthorId = 16, BookId = 15 },
               new BookAuthor { AuthorId = 17, BookId = 15 },
               new BookAuthor { AuthorId = 18, BookId = 16 }
            };
            bookAuthors.ForEach(ba => _context.BookAuthors.Add(ba));
            _context.SaveChanges();

            var authors = new List<Author>
            {
                new Author { Name = "Robert", Surname= "Kiyosaki" },
                new Author { Name = "Ray", Surname = "Dalio" },
                new Author { Name = "Roald", Surname = "Dahl" },
                new Author { Name = "Marget", Surname = "Craven" },
                new Author { Name = "Goerge", Surname = "Orwell" },
                new Author { Name = "M.Scott", Surname = "Peck" },
                new Author { Name = "Gene", Surname = "Roberts" },
                new Author { Name = "Hank", Surname = "Klibanoff" },
                new Author { Name = "Jennet", Surname = "Conant" },
                new Author { Name = "Thomas" , Surname = "Nagel" },
                new Author { Name = "Lysa", Surname =  "TerKeurst" },
                new Author { Name = "Walter", Surname =  "Isaacson" },
                new Author { Name = "Doris Kearns", Surname = "Goodwin" },
                new Author { Name = "Elizabeth", Surname = "MacLeod " },
                new Author { Name = "Bob", Surname = "Woodward" },
                new Author { Name = "Carl", Surname = "Bernstein" },
                new Author { Name = "Pieter-Louis", Surname = "Myburgh" }
            };
            authors.ForEach(a => _context.Authors.Add(a));
            _context.SaveChanges();

        }
    }
}
