using System;
using System.Collections.Generic;

namespace ProkatApp.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public int PasportSeriya { get; set; }

    public int PasportNumber { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public int AddresIndex { get; set; }

    public string? AddresTittle { get; set; }

    public int UserDataId { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual UserDatum UserData { get; set; } = null!;
}
