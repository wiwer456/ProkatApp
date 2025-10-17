using Avalonia.Controls;
using Avalonia.Interactivity;
using ProkatApp.Context;
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
        private void LoginBtn_click(object? s, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ProkatContext context = new ProkatContext();

            var user = context.UserData.Include(x=>x.Staff).FirstOrDefault(x => x.Login == loginTextBox.Text && x.Password == passwordTextBox.Text);
            

            if (user != null) 
            {
                passwordTextBox.Text = "найден";
                Window window = new Menu(context.Staff.First(x=>x.UserDataId == user.UserDataId));
                window.Show();
                this.Close();
                
            }
            else 
            {
                passwordTextBox.Text = "не найден";
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