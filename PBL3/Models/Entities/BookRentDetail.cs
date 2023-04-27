using System;
using System.Collections.Generic;

namespace PBL3.Models.Entities;

public partial class BookRentDetail
{
    public int IdBookRental { get; set; }

    public string IdBook { get; set; } = null!;

    public bool? StateReturn { get; set; }

    public bool? StateApprove { get; set; }

    public bool? StateTake { get; set; }

    public DateTime? ReturnDate { get; set; }

    public virtual Book IdBookNavigation { get; set; } = null!;

    public virtual BookRental IdBookRentalNavigation { get; set; } = null!;
}
