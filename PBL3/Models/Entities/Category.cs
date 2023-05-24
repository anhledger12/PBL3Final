using System;
using System.Collections.Generic;

namespace PBL3.Models.Entities;

public partial class Category
{
    public int Id { get; set; }

    public string? NameCategory   { get; set; }

    public virtual ICollection<Title> IdTitles { get; set; } = new List<Title>();
}
