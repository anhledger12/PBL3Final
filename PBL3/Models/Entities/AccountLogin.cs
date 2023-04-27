using System;
using System.Collections.Generic;

namespace PBL3.Models.Entities;

public partial class AccountLogin
{
    public string AccName { get; set; } = null!;

    public string? PassHashCode { get; set; }

    public bool? Permission { get; set; }

    public virtual Account AccNameNavigation { get; set; } = null!;
}
