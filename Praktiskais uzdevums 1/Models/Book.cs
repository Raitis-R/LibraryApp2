using System;
using System.Collections.Generic;

namespace Praktiskais_uzdevums_1.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int Year { get; set; }

    public int AuthorId { get; set; }

    public int GenreId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;
}
