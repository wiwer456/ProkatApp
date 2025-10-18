using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ProkatApp.Context;
using ProkatApp.Models;
using System.Linq;

namespace ProkatApp;

public partial class Menu : Window
{
    public Menu()
    {
        InitializeComponent();
    }

    public Menu(Staff user)
    {
        ProkatContext context = new ProkatContext();
        InitializeComponent();
        var data = context.UserData.First(x => x.UserDataId == user.UserDataId);
        var role = context.Roles.First(x => x.RoleId == data.RoleId);
        FioTextBlock.Text = data.Fio;
        userRoleTextBlock.Text = role.RoleName;
    }

    private void configOrdersBtn_Click(object? s, RoutedEventArgs e)
    {
        Window window = new OrderWindow();
        window.Show();
        this.Close();
    }

    private void entranceHistoryBtn_Click(object? sender, RoutedEventArgs e)
    {
        Window window = new LoginHistoryWindow();
        window.Show();
        this.Close();
    }
}