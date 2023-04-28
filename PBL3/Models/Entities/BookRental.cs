using System;
using System.Collections.Generic;

namespace PBL3.Models.Entities;

public partial class BookRental
{
    public int Id { get; set; }

    public string? AccApprove { get; set; }

    public string? AccSending { get; set; }

    public DateTime? TimeCreate { get; set; }
    
    public bool ? StateSend { get; set; }   

    public virtual Account? AccApproveNavigation { get; set; }

    public virtual Account? AccSendingNavigation { get; set; }

    public virtual ICollection<BookRentDetail> BookRentDetails { get; set; } = new List<BookRentDetail>();
}
