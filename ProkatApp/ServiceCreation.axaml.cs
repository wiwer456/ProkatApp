using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using ProkatApp.Context;
using ProkatApp.Models;

namespace ProkatApp;

public partial class ServiceCreation : Window
{
    public ServiceCreation()
    {
        InitializeComponent();
    }

    private async void ShowMBox(string ttl, string desc)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(ttl, desc);
        var result = await box.ShowAsync();
    }

    private async void addService_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ProkatContext context = new ProkatContext();

        if (serviceTittle.Text == null)
        {
            ShowMBox("Ошибка", "Введите название услуги");
            return;
        }
        if (serviceCode.Text == null) 
        {
            ShowMBox("Ошибка", "Введите код услуги");
            return;
        }

        if (decimal.TryParse(serviceCost.Text, out decimal cost)) { }
        else
        {
            ShowMBox("Ошибка", "стоимость введена некорректно");
            return;
        }


        Service service = new Service
        {
            ServiceTittle = serviceTittle.Text,
            ServiceCode = serviceCode.Text,
            CostPerHour = cost,
        };
        context.Services.Add(service);
        context.SaveChanges();

        ShowMBox("Успех", "Услуга создана");
    }

    private void ServicesBackBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var wn = new OrderWindow();
        wn.Show();

        this.Close();
    }
}