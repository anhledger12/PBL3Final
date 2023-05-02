using System;
using System.Collections.Generic;

namespace PBL3.Models.Entities;

public partial class Title
{
    public string IdTitle { get; set; } = null!;

    public int? IdRepublish { get; set; }

    public string? NameBook { get; set; }

    public string? NameWriter { get; set; }

    public string? NameBookshelf { get; set; }

    public int? ReleaseYear { get; set; }

    public string? Publisher { get; set; }
    //do not use this attribute
    public DateTime? ReleaseDate { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();

    public virtual Republish? IdRepublishNavigation { get; set; }

    public virtual ICollection<Hashtag> IdHashtags { get; set; } = new List<Hashtag>();
    //new line
    public virtual ICollection<Cart> Carts { get; set; } = new HashSet<Cart>();
}
