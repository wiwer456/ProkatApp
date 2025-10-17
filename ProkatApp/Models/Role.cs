using System;
using System.Collections.Generic;

namespace ProkatApp.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<UserDatum> UserData { get; set; } = new List<UserDatum>();
}
