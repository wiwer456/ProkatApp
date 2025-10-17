using System;
using System.Collections.Generic;

namespace ProkatApp.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string OrderCode { get; set; } = null!;

    public DateOnly DateCreate { get; set; }

    public TimeOnly TimeCreate { get; set; }

    public int ClientId { get; set; }

    public int OrderStatusId { get; set; }

    public DateOnly? DateClose { get; set; }

    public TimeOnly RentTime { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual OrderStatus OrderStatus { get; set; } = null!;

    public virtual ICollection<UserService> UserServices { get; set; } = new List<UserService>();
}
