using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using ProkatApp.Context;
using ProkatApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ProkatApp;

public partial class OrderWindow : Window
{
    public OrderWindow()
    {
        InitializeComponent();
        GetServices();
    }

    private async Task GetServices()
    {
        ProkatContext con = new ProkatContext();
        ServiceComboBox.ItemsSource = con.Services.ToList();
        ClientComboBox.ItemsSource = con.UserData.OrderBy(x => x.Fio).Where(x => x.RoleId == 4).ToList();

    }
    public List<int> services = new List<int>();
    private void ServiceCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        services.Add((ServiceComboBox.SelectedItem as Service).ServiceId);
        listServicesTextBox.Text += $"{(ServiceComboBox.SelectedItem as Service).ServiceId} ";
    }
    
    private void clearServicesBtn_click(object? s, Avalonia.Interactivity.RoutedEventArgs e)
    {
        services.Clear();
        listServicesTextBox.Text = "";
        foreach (var service in services)
        {
            listServicesTextBox.Text += $"{(ServiceComboBox.SelectedItem as Service).ServiceId} ";
        }

    }

    private void addOrderBtn_click(object? s, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ProkatContext con = new ProkatContext();
        string code = "123";
        int hours = int.Parse(rentTimeTextBox.Text) / 60;
        int minutes = int.Parse(rentTimeTextBox.Text) % 60;

        if (orderCodeTextBox.Text != null)
        {
            code = orderCodeTextBox.Text;
        }
        else
        {
            code = $"{(ClientComboBox.SelectedItem as Client)!.ClientId}/{DateOnly.FromDateTime(DateTime.Now)}";
            orderCodeTextBox.Text = code;
        }
        rentTimeTextBox.Text = (ClientComboBox.SelectedItem as Client)!.ClientId.ToString();
        /*Order order = new Order()
        {
            OrderCode = code,
            OrderId = con.Orders.OrderBy(x=>x.OrderId).LastOrDefault().OrderId + 1,
            DateCreate = DateOnly.FromDateTime(DateTime.Now),
            TimeCreate = TimeOnly.FromDateTime(DateTime.Now),
            ClientId = (ClientComboBox.SelectedItem as Client)!.ClientId,
            OrderStatusId = 1,
            DateClose = null,
            RentTime = new TimeOnly(hours, minutes)
        };
        con.Orders.Add(order);
        con.SaveChanges();

        for (int i = 0; i < services.Count; i++)
        {
            UserService userService = new UserService()
            {
                UsId = con.UserServices.OrderBy(u => u.UsId).LastOrDefault().UsId + 1,
                OrderId = con.Orders.OrderBy(x => x.OrderId).LastOrDefault().OrderId,
                ServiceId = services[i]
            };
            con.UserServices.Add(userService);
            con.SaveChanges();
        }*/

    }
}