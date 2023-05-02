using System;
using System.Collections.Generic;

namespace PBL3.Models.Entities;

public partial class Book
{
    public string IdBook { get; set; } = null!;

    public string IdTitle { get; set; }

    public bool? StateRent { get; set; }

    public virtual ICollection<BookRentDetail> BookRentDetails { get; set; } = new List<BookRentDetail>();

    public virtual Title? IdTitleNavigation { get; set; }
}
