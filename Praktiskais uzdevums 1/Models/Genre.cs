using System;
using System.Collections.Generic;

namespace Praktiskais_uzdevums_1.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
