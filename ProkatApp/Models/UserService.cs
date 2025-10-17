using System;
using System.Collections.Generic;

namespace ProkatApp.Models;

public partial class UserService
{
    public int UsId { get; set; }

    public int OrderId { get; set; }

    public int ServiceId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
