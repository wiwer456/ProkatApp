using System;
using System.Collections.Generic;

namespace ProkatApp.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public int UserDataId { get; set; }

    public DateTime LastEntrance { get; set; }

    public int? EntranceStatusId { get; set; }

    public virtual EntranceStatus? EntranceStatus { get; set; }

    public virtual UserDatum UserData { get; set; } = null!;
}
