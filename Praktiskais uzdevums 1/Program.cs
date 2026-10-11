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
    Console.WriteLine("!LINQ ar ievadi:");
    Console.WriteLine("7. Meklēt grāmatas pēc autora vārda");
    Console.WriteLine("8. Grāmatas, izdotas pēc norādītā gadā");
    Console.WriteLine("9. Meklēt grāmatas pēc žanra");
    Console.WriteLine("!LINQ bez ievades:");
    Console.WriteLine("10. Grāmatas sakārtotas pēc gada");
    Console.WriteLine("11. Grāmatu skaits katram autoram");
    Console.WriteLine("12. Vecākā un jaunākā grāmata");
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
        case "7": Query1(); break;
        case "8": Query2(); break;
        case "9": Query3(); break;
        case "10": Query4(); break;
        case "11": Query5(); break;
        case "12": Query6(); break;
        case "0": return;
        default: Console.WriteLine("Nepareiza izvēle."); break;
    }
}

void Query1()
{
    Console.Write("Autora vārds (vai daļa): ");
    var text = Console.ReadLine() ?? "";
    var result = db.Books
        .Include(b => b.Author).Include(b => b.Genre)
        .Where(b => b.Author.Name.Contains(text))
        .ToList();
    PrintBooks(result);
}

void Query2()
{
    Console.Write("Gads (rādīt grāmatas, izdotas pēc šī gada): ");
    if (!int.TryParse(Console.ReadLine(), out int year)) { Console.WriteLine("Nederīgs gads."); return; }
    var result = db.Books
        .Include(b => b.Author).Include(b => b.Genre)
        .Where(b => b.Year == year)
        .OrderBy(b => b.Year)
        .ToList();
    PrintBooks(result);
}

void Query3()
{
    Console.Write("Žanrs (vai daļa): ");
    var text = Console.ReadLine() ?? "";
    var result = db.Books
        .Include(b => b.Author).Include(b => b.Genre)
        .Where(b => b.Genre.Title.Contains(text))
        .ToList();
    PrintBooks(result);
}

void Query4()
{
    var result = db.Books
        .Include(b => b.Author).Include(b => b.Genre)
        .OrderBy(b => b.Year)
        .ToList();
    PrintBooks(result);
}

void Query5()
{
    var result = db.Authors
        .Select(a => new { a.Name, Count = a.Books.Count() })
        .OrderByDescending(x => x.Count)
        .ToList();
    foreach (var x in result)
        Console.WriteLine($"{x.Name}: {x.Count} grāmata(s)");
}

void Query6()
{
    if (!db.Books.Any()) { Console.WriteLine("Nav grāmatu."); return; }
    var oldest = db.Books.Include(b => b.Author).OrderBy(b => b.Year).First();
    var newest = db.Books.Include(b => b.Author).OrderByDescending(b => b.Year).First();
    Console.WriteLine($"Vecākā: {oldest.Title} ({oldest.Year}) - {oldest.Author.Name}");
    Console.WriteLine($"Jaunākā: {newest.Title} ({newest.Year}) - {newest.Author.Name}");
}

void PrintBooks(List<Book> books)
{
    if (books.Count == 0) { Console.WriteLine("Nekas nav atrasts."); return; }
    foreach (var b in books)
        Console.WriteLine($"{b.Id}. {b.Title} ({b.Year}) - {b.Author.Name}, {b.Genre.Title}");
}

void ShowBooks()
{
    PrintBooks(db.Books.Include(b => b.Author).Include(b => b.Genre).ToList());
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