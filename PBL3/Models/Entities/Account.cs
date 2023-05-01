using System;
using System.Collections.Generic;

namespace PBL3.Models.Entities;

public partial class Account
{
    public string AccName { get; set; } = null!;

    public string? FullName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Mssv { get; set; }

    public virtual AccountLogin? AccountLogin { get; set; }

    public virtual ICollection<ActionLog> ActionLogs { get; set; } = new List<ActionLog>();

    public virtual ICollection<BookRental> BookRentalAccApproveNavigations { get; set; } = new List<BookRental>();

    public virtual ICollection<BookRental> BookRentalAccSendingNavigations { get; set; } = new List<BookRental>();

    public virtual ICollection<Notificate> Notificates { get; set; } = new List<Notificate>();
    //new line
    public virtual ICollection<Cart> Carts { get; set; } = new HashSet<Cart>();
}
