using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MsBox.Avalonia;
using ProkatApp.Context;
using ProkatApp.Models;
using ProkatApp.Properties;
using SkiaSharp;
using System;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using Tmds.DBus.Protocol;

namespace ProkatApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void ShowMBox(string ttl, string desc)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(ttl, desc);
            var result = await box.ShowAsync();
        }
        private LoginHistory CreateLogHistory(int stat)
        {
            ProkatContext context = new ProkatContext();
            var user = context.UserData.Include(x => x.Staff).FirstOrDefault(x => x.Login == loginTextBox.Text);
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
            int attempt = 1;
            if (loginTextBox.Text == null)
            {
                ShowMBox("Ошибка", "Введите логин");
                return;
            }
            if (passwordTextBox.Text == null)
            {
                ShowMBox("Ошибка", "Введите пароль");
                return;
            }

            /*if (Settings.Default.attempt_count > 0)
            {

            }*/
            if (Settings.Default.attempt_count < 2)
            {
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
                    ShowMBox("Ошибка", "Пользователь не найден");
                    Settings.Default.attempt_count++;
                    //context.LoginHistories.Add(CreateLogHistory(2));
                    attempt += attempt;
                    context.SaveChanges();
                }
            }
            else if (Settings.Default.attempt_count >= 2)
            {
                CaptchaGrid.IsVisible = true;
                CaptchaImg.IsVisible = true;
                Settings.Default.attempt_count = 0;
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

        public string GenerateRandomString(int len)
        {
            string chars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";
            /*string chars = "ЁЙЦУКЕНГШЩЗФЫВАПРОЛДЯЧСМИТЬйцукенгшщзхъфывапролджэячсмитьбюQWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";*/
            string result = "";
            for (int i = 0; i < len; i++)
            {
                result += chars[RandomNumberGenerator.GetInt32(chars.Length)];
            }
            return result;
        }

       
    }
}