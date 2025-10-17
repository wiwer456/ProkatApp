using System;
using System.Collections.Generic;

namespace ProkatApp.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string ServiceTittle { get; set; } = null!;

    public string ServiceCode { get; set; } = null!;

    public decimal CostPerHour { get; set; }

    public virtual ICollection<UserService> UserServices { get; set; } = new List<UserService>();
}
