using System;
using System.Collections.Generic;

namespace ProkatApp.Models;

public partial class LoginHistory
{
    public int LoginHistoryId { get; set; }

    public int UserDataId { get; set; }

    public DateTime LoginTime { get; set; }

    public int EntranceStatusId { get; set; }

    public virtual EntranceStatus EntranceStatus { get; set; } = null!;

    public virtual UserDatum UserData { get; set; } = null!;
}
