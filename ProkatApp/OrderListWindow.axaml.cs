using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using ProkatApp.Context;
using ProkatApp.Models;
using ProkatApp.Properties;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ProkatApp;

public partial class OrderListWindow : Window
{
    public OrderListWindow()
    {
        InitializeComponent();
        LoadOrdrs();
        DataContext = this;
    }

    public async void ShowMsBox(string title, string msg)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(title, msg);
        var result = await box.ShowAsync();
    }

    public class OrderTemplate
    {
        public int orderId { get; set; }
        public string orderCode { get; set; }
        public string orderDateTime { get; set; }
        public string orderClient { get; set; }
        public string orderStatus { get; set; }
        public string orderRentTime { get; set; }
        public string orderDateClose { get; set; }
    }

    public ObservableCollection<OrderTemplate> orderTemplates { get; set; }
    public OrderTemplate selectedOrder { get; set; }

    public void LoadOrdrs()
    {
        ProkatContext context = new ProkatContext();

        orderTemplates = new ObservableCollection<OrderTemplate>(context.Orders.Include(o => o.Client)
            .Select(o => new OrderTemplate
            {
                orderId = o.OrderId,
                orderCode = o.OrderCode,
                orderDateTime = o.DateCreate.ToString() + "\n" + o.TimeCreate.ToString(),
                orderClient = o.Client.UserData.Fio,
                orderStatus = o.OrderStatus.StatusTittle,
                orderRentTime = o.RentTime.ToString("HH:mm"),
                orderDateClose = string.IsNullOrWhiteSpace(o.DateClose.ToString()) ? "---" : o.DateClose.ToString()
            })
            .OrderByDescending(o => o.orderId)
            .ToList());

        OrdersListbox.ItemsSource = orderTemplates;
    }

    private async void OrderCloseBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ProkatContext context = new ProkatContext();
        if (selectedOrder != null)
        {
            Order order = context.Orders.Where(o => o.OrderId == selectedOrder.orderId).FirstOrDefault();
            if (order.OrderStatusId == 3)
            {
                ShowMsBox("Ошибка", "Заказ уже закрыт!");
            }
            else
            {
                order.OrderStatusId = 3;
                order.DateClose = DateOnly.FromDateTime(DateTime.Now);
                context.Orders.Update(order);
                context.SaveChanges();
                ShowMsBox("Успех", "Заказ закрыт");
            }
            LoadOrdrs();
        }
        else
        {
            ShowMsBox("Ошибка", "Выберите заказ");
        }
    }

    private void OrderBackBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ProkatContext contetx = new ProkatContext();
        Window menu = new Menu(contetx.Staff.First(s => s.UserDataId == Settings.Default.UserD_Id));
        menu.Show();
        this.Close();
    }

    private async void OrderConfirmBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ProkatContext context = new ProkatContext();
        if (selectedOrder != null)
        {
            Order order = context.Orders.Where(o => o.OrderId == selectedOrder.orderId).FirstOrDefault();
            if (order.OrderStatusId == 3)
            {
                ShowMsBox("Ошибка", "Заказ закрыт!");
            }
            else if (order.OrderStatusId == 2)
            {
                ShowMsBox("Ошибка", "Заказ уже находится в прокате!");
            }
            else
            {
                order.OrderStatusId = 2;
                context.Orders.Update(order);
                context.SaveChanges();
                ShowMsBox("Успех", "Заказ принят в прокат");
            }
            LoadOrdrs();
        }
        else
        {
            ShowMsBox("Ошибка", "Выберите заказ");
        }
    }
}