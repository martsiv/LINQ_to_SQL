using System.Data.Linq;

namespace _01_LINQ_to_SQL_Library
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Library;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
            LibraryDataContext context = new LibraryDataContext(conn);
            var authors = context.Authors;
            var books = context.Books;
            var countries = context.Countries;
            //1
            Console.WriteLine("\nBooks by pages");
            foreach ( var book in BooksByPageCount(context, 25) )
                Console.WriteLine($"{book.Name} - {book.Author}, {book.Pages}");
            context.SubmitChanges();
            //2
            string letter = "a";
            Console.WriteLine($"\nBooks by letter - {letter}");
            foreach (var book in BooksByFirstLetter(context, letter))
                Console.WriteLine($"{book.Name} - {book.Author}, {book.Pages}");
            context.SubmitChanges();
            //3
            string name = "John";
            string surname = "Smith";
            Console.WriteLine($"\nBooks by author {name} {surname}");
            foreach (var book in BooksByAuthor(context, name, surname))
                Console.WriteLine($"{book.Name} - {book.Author}, {book.Pages}");
            context.SubmitChanges();
            //4
            string country = "United States";
            Console.WriteLine($"\nBooks by country - {country}");
            foreach (var book in BooksByCountryAndSort(context, country))
                Console.WriteLine($"{book.Name} - {book.Author}, {book.Pages}");
            context.SubmitChanges();
            //5
            int count = 10;
            Console.WriteLine($"\nBooks by name count - {count}");
            foreach (var book in BooksByNameCount(context, count))
                Console.WriteLine($"{book.Name} - {book.Author}, {book.Pages}");
            context.SubmitChanges();
            //6
            
            Console.WriteLine($"\nTop pages book in some country ({country})");
            if (TopBookByMaxPagesInSomeCountry(context, country) is Book topBook)
                Console.WriteLine($"{topBook.Name} - {topBook.Author}, {topBook.Pages}");
            context.SubmitChanges();
            //7
            Console.WriteLine("\nAuthor with less books in DB");
            if (AuthorWithLessBooksInDB(context) is Author authorWithLessBooks)
                Console.WriteLine(authorWithLessBooks);
            context.SubmitChanges();
            //8
            Console.WriteLine("\nCountry with most authors count in DB");
            if (CountryWithMostAuthors(context) is Country countryMostAuthor)
                Console.WriteLine(countryMostAuthor);
            context.SubmitChanges();
        }
        static List<Book> BooksByPageCount(LibraryDataContext context, int pages)
        {
            var books = context.Books.Where(x => pages < x.Pages);
            return books.ToList();
        }

        static List<Book> BooksByFirstLetter(LibraryDataContext context, string letter)
        {
            //var res = from books in context.Books
            //            where books.Name.StartsWith(letter)
            //            select books;
            var res = context.Books.Where(x => x.Name.StartsWith(letter));
            return res.ToList();
        }

        static List<Book> BooksByAuthor(LibraryDataContext context, string firstname, string lastname)
        {
            //var res = from books in context.Books
            //          where books.Author.Name == firstname && books.Author.Surname == lastname
            //          select books;
            var res = context.Books.Where(x => x.Author.Name == firstname && x.Author.Surname == lastname);
            return res.ToList();
        }

        static List<Book> BooksByCountryAndSort(LibraryDataContext context, string country)
        {
            var res = context.Books.Where(x => x.Author.Country.Name == country).OrderBy(x => x.Name);
            return res.ToList();
        }

        static List<Book> BooksByNameCount(LibraryDataContext context, int count)
        {
            var res = context.Books.Where(x => x.Name.Length == count);
            return res.ToList();
        }

        static Book? TopBookByMaxPagesInSomeCountry(LibraryDataContext context, string country)
        {
            var res = context.Books.Where(x => x.Author.Country.Name == country).OrderByDescending(x => x.Pages).FirstOrDefault();
            return res;
        }

        static Author? AuthorWithLessBooksInDB(LibraryDataContext context)
        {
            var res = context.Authors.FirstOrDefault(x => x.Books.Count == context.Authors.Min(x => x.Books.Count()));
            return res;
        }

        static Country? CountryWithMostAuthors(LibraryDataContext context)
        {
            int mostOuthorsInCountry = context.Countries.Max(x => x.Authors.Count());
            var res = context.Countries.FirstOrDefault(x => x.Authors.Count == mostOuthorsInCountry);
            return res;
        }
    }
}