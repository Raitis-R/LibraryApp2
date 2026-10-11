using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Praktiskais_uzdevums_1.Models;

[Table("Authors")]
public partial class Author
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Autora vārds ir obligāts")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}