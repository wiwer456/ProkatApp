using System;
using System.Collections.Generic;

namespace ProkatApp.Models;

public partial class UserDatum
{
    public int UserDataId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Fio { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
