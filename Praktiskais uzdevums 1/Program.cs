using Microsoft.EntityFrameworkCore;
using Praktiskais_uzdevums_1.Models;

Console.OutputEncoding = System.Text.Encoding.UTF8;
using var db = new LibraryDbContext();

while (true)
{
    Console.WriteLine("\n Bibliotēka");
    Console.WriteLine("1. Rādīt grāmatas");
    Console.WriteLine("2. Pievienot grāmatu");
    Console.WriteLine("3. Rādīt autorus");
    Console.WriteLine("4. Pievienot autoru");
    Console.WriteLine("5. Rādīt žanrus");
    Console.WriteLine("6. Pievienot žanru");
    Console.WriteLine("0. Iziet");
    Console.Write("Izvēle: ");

    switch (Console.ReadLine())
    {
        case "1": ShowBooks(); break;
        case "2": AddBook(); break;
        case "3": ShowAuthors(); break;
        case "4": AddAuthor(); break;
        case "5": ShowGenres(); break;
        case "6": AddGenre(); break;
        case "0": return;
        default: Console.WriteLine("Nepareiza izvēle."); break;
    }
}

void ShowBooks()
{
    var books = db.Books.Include(b => b.Author).Include(b => b.Genre).ToList();
    foreach (var b in books)
        Console.WriteLine($"{b.Id}. {b.Title} ({b.Year}) - {b.Author.Name}, {b.Genre.Title}");
}

void ShowAuthors()
{
    foreach (var a in db.Authors.ToList())
        Console.WriteLine($"{a.Id}. {a.Name}");
}

void ShowGenres()
{
    foreach (var g in db.Genres.ToList())
        Console.WriteLine($"{g.Id}. {g.Title}");
}

void AddAuthor()
{
    Console.Write("Autora vārds: ");
    db.Authors.Add(new Author { Name = Console.ReadLine() ?? "" });
    db.SaveChanges();
    Console.WriteLine("Autors pievienots.");
}

void AddGenre()
{
    Console.Write("Žanra nosaukums: ");
    db.Genres.Add(new Genre { Title = Console.ReadLine() ?? "" });
    db.SaveChanges();
    Console.WriteLine("Žanrs pievienots.");
}

void AddBook()
{
    Console.Write("Nosaukums: ");
    var title = Console.ReadLine() ?? "";

    Console.Write("Izdošanas gads: ");
    if (!int.TryParse(Console.ReadLine(), out int year)) { Console.WriteLine("Nederīgs gads."); return; }

    ShowAuthors();
    Console.Write("Autora Id: ");
    if (!int.TryParse(Console.ReadLine(), out int authorId) || !db.Authors.Any(a => a.Id == authorId))
    { Console.WriteLine("Autors nav atrasts."); return; }

    ShowGenres();
    Console.Write("Žanra Id: ");
    if (!int.TryParse(Console.ReadLine(), out int genreId) || !db.Genres.Any(g => g.Id == genreId))
    { Console.WriteLine("Žanrs nav atrasts."); return; }

    db.Books.Add(new Book { Title = title, Year = year, AuthorId = authorId, GenreId = genreId });
    db.SaveChanges();
    Console.WriteLine("Grāmata pievienota.");
}