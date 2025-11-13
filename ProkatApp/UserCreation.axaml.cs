using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using ProkatApp.Models;
using ProkatApp.Context;
using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace ProkatApp;

public partial class UserCreation : Window
{
    public UserCreation()
    {
        InitializeComponent();
    }

    private async void ShowMBox(string ttl, string desc)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(ttl, desc);
        var result = await box.ShowAsync();
    }

    private async void AddClient_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ProkatContext context = new ProkatContext();

        if (string.IsNullOrWhiteSpace(FIOTextBox.Text))
        {
            ShowMBox("Ошибка", "Введите ФИО пользователя");
        }
        if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
        {
            ShowMBox("Ошибка", "Введите почту пользователя");
        }
        if (string.IsNullOrWhiteSpace(PasNumberTextBox.Text))
        {
            ShowMBox("Ошибка", "Введите пароль пользователя");
        }

        if (int.TryParse(PasSeriyaTextBox.Text, out int seriya)) { }
        else
        {
            ShowMBox("Ошибка", "Серия не введена или введена некорректно");
            return;
        }
        if (int.TryParse(PasNumberTextBox.Text, out int num)) { }
        else
        {
            ShowMBox("Ошибка", "Номер паспорта не введён или введён некорректно");
            return;
        }
        if (int.TryParse(IndexTextBox.Text, out int index)) { }
        else
        {
            ShowMBox("Ошибка", "Индекс не введён или введён некорректно");
            return;
        }

        DateOnly dateBirth = DateOnly.FromDateTime(
            DateBirthPicker.SelectedDate?.Date ?? DateTime.Now
        );

        UserDatum UData = new UserDatum
        {
            UserDataId = context.UserData.OrderBy(u => u.UserDataId).LastOrDefault().UserDataId + 1,
            RoleId = 4,
            Fio = FIOTextBox.Text,
            Login = EmailTextBox.Text,
            Password = PasswordTextBox.Text,

        };
        context.UserData.Add(UData);
        context.SaveChanges();

        Client client = new Client
        {
            ClientId = context.Clients.OrderBy(x => x.ClientId).LastOrDefault().ClientId + 1,
            PasportSeriya = seriya,
            PasportNumber = num,
            AddresIndex = index,
            DateOfBirth = dateBirth,
            AddresTittle = AdresTextBox.Text,
            UserDataId = context.UserData.OrderBy(u => u.UserDataId).LastOrDefault().UserDataId

        };
        context.Clients.Add(client);
        context.SaveChanges();

        ShowMBox("Успех", "Пользователь успешно добавлен");
    }

    private void UserCBackBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var wn = new OrderWindow();
        wn.Show();

        this.Close();
    }
}

