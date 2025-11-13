using Avalonia.Controls;
using Avalonia.Interactivity;
using MsBox.Avalonia;
using ProkatApp.Context;
using ProkatApp.Models;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using Tmds.DBus.Protocol;
using ProkatApp.Properties;

namespace ProkatApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private LoginHistory CreateLogHistory(int stat)
        {
            ProkatContext context = new ProkatContext();
            var user = context.UserData.Include(x => x.Staff).FirstOrDefault(x => x.Login == loginTextBox.Text && x.Password == passwordTextBox.Text);
            if (user != null)
            {
                LoginHistory loginHistory = new LoginHistory
                {
                    UserDataId = user.UserDataId,
                    LoginTime = System.DateTime.Now,
                    EntranceStatusId = stat
                };
                return loginHistory;
            }
            return null;
        }

        private async void LoginBtn_click(object? s, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ProkatContext context = new ProkatContext();
            var user = context.UserData.Include(x=>x.Staff).FirstOrDefault(x => x.Login == loginTextBox.Text && x.Password == passwordTextBox.Text);

            if (user != null) 
            {
                Window window = new Menu(context.Staff.First(x => x.UserDataId == user.UserDataId));
                /*context.LoginHistories.Add(CreateLogHistory(1));*/
                var box = MessageBoxManager.GetMessageBoxStandard("Успех", $"Добро пожаловать, {user.Fio}");
                var result = await box.ShowAsync();
                context.SaveChanges();
                Settings.Default.UserD_Id = user.UserDataId;
                window.Show();
                this.Close();
            }
            else 
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Пользователь не найден");
                var result = await box.ShowAsync();
                context.SaveChanges();
            }
        }
        private void showPasswordBtn_click(object? s, RoutedEventArgs e)
        {
            if (passwordTextBox.RevealPassword == true)
            {
                passwordTextBox.RevealPassword = false;
            }
            else
            {
                passwordTextBox.RevealPassword = true;
            }
        }

    }
}