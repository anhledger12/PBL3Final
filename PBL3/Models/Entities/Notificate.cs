using System;
using System.Collections.Generic;

namespace PBL3.Models.Entities;

public partial class Notificate
{
    public int Id { get; set; }

    public string? AccReceive { get; set; }

    public DateTime? TimeSending { get; set; }

    public string? Content { get; set; }

    public bool? StateRead { get; set; }

    public virtual Account? AccReceiveNavigation { get; set; }
}
