using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using ProkatApp.Context;
using ProkatApp.Models;
using ProkatApp.Properties;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

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
        orderCodeTextBox.Text = (con.Orders.OrderBy(x => x.OrderId).LastOrDefault().OrderId + 1).ToString();
        Console.WriteLine(orderCodeTextBox.Text);
    }
    public List<int> services = new List<int>();
    public List<decimal> price = new List<decimal>();

    /*private void ServiceCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        services.Add((ServiceComboBox.SelectedItem as Service).ServiceId);
        price.Add((ServiceComboBox.SelectedItem as Service).CostPerHour);
        listServicesTextBox.Text += $"{(ServiceComboBox.SelectedItem as Service).ServiceTittle}\n";
    }*/
    private async void ShowMBox(string ttl, string desc)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(ttl, desc);
        var result = await box.ShowAsync();
    }

    private async void ServiceSelectionBtn_click(object? s, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Service serv = (ServiceComboBox.SelectedItem as Service);
        if (serv != null)
        {
            services.Add(serv.ServiceId);
            price.Add(serv.CostPerHour);
            listServicesTextBox.Text += $"{serv.ServiceTittle} {price.Last()}\n";
        }
        else
        {
            ShowMBox("Ошибка", "Выберите услугу для добавления");
        }
    }

    private void clearServicesBtn_click(object? s, Avalonia.Interactivity.RoutedEventArgs e)
    {
        services.Clear();
        price.Clear();
        listServicesTextBox.Text = "";

    }

    private async void addOrderBtn_click(object? s, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ProkatContext con = new ProkatContext();
        string code = "";
        int hours = 0; /*int.Parse(rentTimeTextBox.Text) / 60;*/
        int minutes = 0;/*int.Parse(rentTimeTextBox.Text) % 60;*/

        /*if (string.IsNullOrWhiteSpace(orderCodeTextBox.Text))
        {
            code = $"{tUser.ClientId}/{DateOnly.FromDateTime(DateTime.Now)}";
            orderCodeTextBox.Text = code;
        }
        else
        {
            code = orderCodeTextBox.Text;
        }*/

        if (string.IsNullOrWhiteSpace(orderCodeTextBox.Text)) 
        {
            ShowMBox("Ошибка", "Пустое поле ввода для кода заказа");
            return;
        } 
        else
        {
            code = orderCodeTextBox.Text;
        }
        if (ClientComboBox.SelectedItem == null) 
        {
            ShowMBox("Ошибка", "Выберите пользователя");
            return;
        }
        if (services.Count == 0)
        {
            ShowMBox("Ошибка", "Выберите Услуги");
            return;
        }
        if (int.TryParse(rentTimeTextBox.Text, out hours) && int.TryParse(rentTimeTextBox.Text, out minutes))
        {
            hours = hours / 60;
            minutes = minutes % 60;
        }
        else
        {
            ShowMBox("Ошибка", "Время аренды введено некорректно");
            return;
        }
        int uid = (ClientComboBox.SelectedItem as UserDatum)!.UserDataId;
        var tUser = con.Clients.OrderBy(c => c.ClientId).Where(c => c.UserDataId == uid).FirstOrDefault();
        List<string> orderCodes = new List<string>(con.Orders.Select(o => o.OrderCode).ToList());

        foreach (string c in orderCodes)
        {
            if (orderCodeTextBox.Text == c)
            {
                ShowMBox("Ошибка", "Запись уже существует");
                return;
            }
        }
        Order order = new Order()
        {
            OrderCode = code,
            OrderId = con.Orders.OrderBy(x => x.OrderId).LastOrDefault().OrderId + 1,
            DateCreate = DateOnly.FromDateTime(DateTime.Now),
            TimeCreate = TimeOnly.FromDateTime(DateTime.Now),
            ClientId = tUser.ClientId,
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
        }

        ShowMBox("Успех", "Заказ добавлен");
        /*orderCodeTextBox.Text = code + 1;*/
    }

    private void OrderBackBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ProkatContext context = new ProkatContext();
        Window menu = new Menu(context.Staff.First(s => s.UserDataId == Settings.Default.UserD_Id));
        menu.Show();
        this.Close();
    }

    private void AddClientBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Window add = new UserCreation();
        add.Show();
        this.Close();
    }

    private void CreateServiceBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Window add = new ServiceCreation();
        add.Show();
        this.Close();
    }
}