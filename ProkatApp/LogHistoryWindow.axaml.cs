using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using ProkatApp.Context;
using ProkatApp.Models;
using ProkatApp.Properties;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;

namespace ProkatApp;

public partial class LogHistoryWindow : Window
{
    public LogHistoryWindow()
    {
        InitializeComponent();
        LoadHistory();
        ApplyFilters();
        DataContext = this;
        FiltersComboBox.SelectedIndex = 0;
    }

    public ObservableCollection<HistoryRow> historyRows { get; set; }
    private ObservableCollection<HistoryRow> _allHistoryRows { get; set; }
    public class HistoryRow
    {
        public string loginTime { get; set; }
        public string login { get; set; }
        public string loginStatus { get; set; }
    }
    public void LoadHistory()
    {
        var context = new ProkatContext();
        _allHistoryRows = new ObservableCollection<HistoryRow>(context.LoginHistories.Include(u => u.UserData).Include(u => u.EntranceStatus)
            .Select(u => new HistoryRow
            {
                loginTime = u.LoginTime.ToString("yyyy-MM-dd HH:mm"),
                login = u.UserData.Login,
                loginStatus = u.EntranceStatus.StatusTittle
            })
            .ToList());
        historyRows = new ObservableCollection<HistoryRow>(context.LoginHistories.Include(u => u.UserData).Include(u => u.EntranceStatus)
            .Select(u => new HistoryRow
            {
                loginTime = u.LoginTime.ToString(),
                login = u.UserData.Login,
                loginStatus = u.EntranceStatus.StatusTittle
            })
            .ToList());
    }

    public async void ApplyFilters()
    {
        var temp = _allHistoryRows.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
        {
            temp = temp.Where(t => t.login.ToLower().Contains(SearchTextBox.Text.ToLower()));
        }
        int index = FiltersComboBox.SelectedIndex;
        if (index == 0)
        {
            temp = temp.OrderBy(t => t.loginTime);
        }
        else if (index == 1)
        {
            temp = temp.OrderByDescending(t => t.loginTime);

        }

        historyRows.Clear();
        foreach (var t in temp)
        {
            historyRows.Add(t);
        }
        HistoryListBox.ItemsSource = historyRows;
    }

    private void FiltersComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void SearchTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        ApplyFilters();
    }

    private void HistoryBackBtn_Click(object? s, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ProkatContext context = new ProkatContext();
        Window menu = new Menu(context.Staff.First(s => s.UserDataId == Settings.Default.UserD_Id));
        menu.Show();
        this.Close();
    }
}