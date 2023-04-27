using System;
using System.Collections.Generic;

namespace PBL3.Models.Entities;

public partial class Hashtag
{
    public int Id { get; set; }

    public string? NameHashTag { get; set; }

    public virtual ICollection<Title> IdTitles { get; set; } = new List<Title>();
}
