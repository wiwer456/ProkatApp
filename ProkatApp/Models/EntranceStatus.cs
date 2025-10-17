using System;
using System.Collections.Generic;

namespace ProkatApp.Models;

public partial class EntranceStatus
{
    public int EntranceStatusId { get; set; }

    public string StatusTittle { get; set; } = null!;

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
