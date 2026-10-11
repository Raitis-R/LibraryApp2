using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Praktiskais_uzdevums_1.Models;

[Table("Books")]
public partial class Book
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Nosaukums ir obligāts")]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; set; } = null!;

    [Range(1000, 2100, ErrorMessage = "Gads jāievada no 1000 līdz 2100")]
    public int Year { get; set; }

    public int AuthorId { get; set; }

    public int GenreId { get; set; }

    [ForeignKey("AuthorId")]
    public virtual Author Author { get; set; } = null!;

    [ForeignKey("GenreId")]
    public virtual Genre Genre { get; set; } = null!;
}