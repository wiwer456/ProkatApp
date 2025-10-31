using Avalonia.Controls;
using Avalonia.Interactivity;
using ProkatApp.Context;
using ProkatApp.Models;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using Tmds.DBus.Protocol;

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
            LoginHistory loginHistory = new LoginHistory()
            {

                UserDataId = user.UserDataId,
                LoginTime = System.DateTime.Now,
                EntranceStatusId = stat
            };
            return loginHistory;
        }

        private void LoginBtn_click(object? s, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ProkatContext context = new ProkatContext();
            var user = context.UserData.Include(x=>x.Staff).FirstOrDefault(x => x.Login == loginTextBox.Text && x.Password == passwordTextBox.Text);

            if (user != null) 
            {
                Window window = new Menu(context.Staff.First(x => x.UserDataId == user.UserDataId));
                context.LoginHistories.Add(CreateLogHistory(1));
                context.SaveChanges();
                window.Show();
                this.Close();
            }
            else 
            {
                passwordTextBox.Text = "не найден";
                context.LoginHistories.Add(CreateLogHistory(2));
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