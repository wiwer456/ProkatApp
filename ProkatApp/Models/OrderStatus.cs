using System;
using System.Collections.Generic;

namespace ProkatApp.Models;

public partial class OrderStatus
{
    public int OrderStatusId { get; set; }

    public string StatusTittle { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
