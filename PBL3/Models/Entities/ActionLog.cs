using System;
using System.Collections.Generic;

namespace PBL3.Models.Entities;

public partial class ActionLog
{
    public int Id { get; set; }

    public string? Acc { get; set; }

    public DateTime? Time { get; set; }

    public string? Content { get; set; }

    public virtual Account? AccNavigation { get; set; }
}
