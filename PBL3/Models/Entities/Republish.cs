using System;
using System.Collections.Generic;

namespace PBL3.Models.Entities;

public partial class Republish
{
    public int Id { get; set; }

    public string? NameRepublisher { get; set; }

    public int? NumOfRep { get; set; }

    public DateTime? TimeOfRep { get; set; }

    public virtual ICollection<Title> Titles { get; set; } = new List<Title>();
}
