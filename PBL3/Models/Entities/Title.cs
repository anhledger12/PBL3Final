using System;
using System.Collections.Generic;

namespace PBL3.Models.Entities;

public partial class Title
{
    public string IdTitle { get; set; } = null!;

    public string? NameBook { get; set; }

    public string? NameWriter { get; set; }

    public string? NameBookshelf { get; set; }
    
    public int? ReleaseYear { get; set; }
   
    public string? Publisher { get; set; }

    public bool Active { get; set; } = true;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual ICollection<Hashtag> IdHashtags { get; set; } = new List<Hashtag>();
}
