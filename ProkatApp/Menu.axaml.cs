using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging; 
using ProkatApp.Context;
using ProkatApp.Models;
using ProkatApp.Properties;
using System.IO;
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

        string imagePath = Path.Combine("Resources", "Images", user.ImagePath);
        if (File.Exists(imagePath))
        {
            var bitmap = new Bitmap(imagePath);
            StaffImage.Source = bitmap;
        }

        if (data.RoleId == 1)
        {
            acceptTovarBtn.IsVisible = false;
            entranceHistoryBtn.IsVisible = false;
        }
        if (data.RoleId == 3)
        {
            entranceHistoryBtn.IsVisible = false;
        }
    }

    private void configOrdersBtn_Click(object? s, RoutedEventArgs e)
    {
        Window window = new OrderWindow();
        window.Show();
        this.Close();
    }

    private void entranceHistoryBtn_Click(object? sender, RoutedEventArgs e)
    {
        Window window = new LogHistoryWindow();
        window.Show();
        this.Close();
    }

    private void acceptTovarBtn_Click(object? sender, RoutedEventArgs e)
    {
        Window ol = new OrderListWindow();
        ol.Show();
        this.Close();
    }

    private void MenuBackBtn_Click(object? sender, RoutedEventArgs e)
    {
        Settings.Default.Reset();
        Window auth = new MainWindow();
        auth.Show();
        this.Close();
    }
}