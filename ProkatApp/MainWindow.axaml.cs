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

        private int attempts = 0;
        private async void LoginBtn_click(object? s, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ProkatContext context = new ProkatContext();
            var user = context.UserData.Include(x => x.Staff).FirstOrDefault(x => x.Login == loginTextBox.Text && x.Password == passwordTextBox.Text);
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


            /*if (Settings.Default.attempt_count < 2)
            {
                if (user != null)
                {
                    Window window = new Menu(context.Staff.First(x => x.UserDataId == user.UserDataId));
                    //context.LoginHistories.Add(CreateLogHistory(1));
                    context.SaveChanges();
                    Settings.Default.UserD_Id = user.UserDataId;
                    window.Show();
                    ShowMBox("Успех", $"Добро пожаловать, {user.Fio}");
                    this.Close();
                }
                else
                {
                    ShowMBox("Ошибка", "Пользователь не найден");
                    Settings.Default.attempt_count++;
                    //context.LoginHistories.Add(CreateLogHistory(2));
                    context.SaveChanges();
                    if (Settings.Default.attempt_count == 2)
                    {
                        CaptchaGrid.IsVisible = true;
                        GenerateCaptcha();
                    } 
                }
            }
            else if (Settings.Default.attempt_count >= 2)
            {
                if (user != null && CaptchaTextBox.Text == captchaText)
                {
                    Window window = new Menu(context.Staff.First(x => x.UserDataId == user.UserDataId));
                    //context.LoginHistories.Add(CreateLogHistory(1));
                    Settings.Default.attempt_count = 0;
                    context.SaveChanges();
                    Settings.Default.UserD_Id = user.UserDataId;
                    window.Show();
                    ShowMBox("Успех", $"Добро пожаловать, {user.Fio}");
                    this.Close();
                }
                else if (user == null || CaptchaTextBox.Text != captchaText)
                {
                    ShowMBox("Ошибка", "Вы заблокированны");
                    Settings.Default.attempt_count = 0;
                    CaptchaGrid.IsVisible = false;
                }
            }*/

            switch (attempts)
            {
                case < 2:
                    if (user != null)
                    {
                        Window window = new Menu(context.Staff.First(x => x.UserDataId == user.UserDataId));
                        //context.LoginHistories.Add(CreateLogHistory(1));
                        context.SaveChanges();
                        Settings.Default.UserD_Id = user.UserDataId;
                        window.Show();
                        ShowMBox("Успех", $"Добро пожаловать, {user.Fio}");
                        this.Close();
                        break;
                    }
                    else
                    {
                        ShowMBox("Ошибка", "Пользователь не найден");
                        attempts++;
                        //context.LoginHistories.Add(CreateLogHistory(2));
                        context.SaveChanges();
                        if (attempts == 2)
                        {
                            CaptchaGrid.IsVisible = true;
                            GenerateCaptcha();
                        }
                        break;
                    }
                case 2:
                    if (user != null)
                    {
                        Window window = new Menu(context.Staff.First(x => x.UserDataId == user.UserDataId));
                        //context.LoginHistories.Add(CreateLogHistory(1));
                        attempts = 0;
                        context.SaveChanges();
                        Settings.Default.UserD_Id = user.UserDataId;
                        window.Show();
                        ShowMBox("Успех", $"Добро пожаловать, {user.Fio}");
                        this.Close();
                    }
                    else if (user == null || CaptchaTextBox.Text != captchaText)
                    {
                        ShowMBox("Ошибка", "Вы заблокированны");
                        attempts = 0;
                        CaptchaGrid.IsVisible = false;
                    }
                    break;
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

        public string captchaText;

        public void GenerateCaptcha()
        {
            captchaText = GenerateRandomString(5);
            CaptchaImg.Source = GenerateCaptchaImage(captchaText);
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


        public IImage GenerateCaptchaImage(string text)
        {
            int width = 200;
            int height = 50;

            SKBitmap skBitmap = new SKBitmap(width, height);
            using SKCanvas canvas = new SKCanvas(skBitmap);

            var rnd = new Random();
            canvas.Clear(SKColors.White);

            float x = 10;

            foreach (char c in text)
            {
                float fontSize = rnd.Next(20, 40);
                using var paint = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = fontSize,
                    Typeface = SKTypeface.FromFamilyName("Times New Roman"),
                    IsAntialias = true
                };

                float y = height / 2f + fontSize / 2f;

                canvas.Save();
                canvas.Translate(x, y);
                canvas.RotateDegrees((float)(rnd.NextDouble() * 12 - 6));
                canvas.DrawText(c.ToString(), 0, 0, paint);
                canvas.Restore();

                x += paint.MeasureText(c.ToString()) + rnd.Next(2, 10);
            }

            // Шум линии
            for (int i = 0; i < RandomNumberGenerator.GetInt32(20); i++)
            {
                using var pen = new SKPaint
                {
                    Color = new SKColor(
                        (byte)RandomNumberGenerator.GetInt32(255),
                        (byte)RandomNumberGenerator.GetInt32(255),
                        (byte)RandomNumberGenerator.GetInt32(255)
                    ),
                    StrokeWidth = 1
                };

                canvas.DrawLine(
                    new SKPoint(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)),
                    new SKPoint(RandomNumberGenerator.GetInt32(width), RandomNumberGenerator.GetInt32(height)),
                    pen
                );
            }

            using var ms = new MemoryStream();
            skBitmap.Encode(ms, SKEncodedImageFormat.Png, 100);
            ms.Seek(0, SeekOrigin.Begin);
            var avaloniaBitmap = new Bitmap(ms);

            return avaloniaBitmap;
        }

        private void CaptchaButton_Click(object? sender, RoutedEventArgs e)
        {
            GenerateCaptcha();
        }
    }
}