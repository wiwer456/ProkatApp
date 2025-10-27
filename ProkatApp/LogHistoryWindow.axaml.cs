using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProkatApp.Models;
using ProkatApp.Context;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;

namespace ProkatApp;

public partial class LogHistoryWindow : Window
{
    public LogHistoryWindow()
    {
        InitializeComponent();
        LoadHistory();
        DataContext = this;

    }

    public ObservableCollection<HistoryRow> historyRows { get; set; }
    public class HistoryRow
    {
        public string loginTime { get; set; }
        public string login { get; set; }
        public string loginStatus { get; set; }
    }
    public void LoadHistory(string? sort = null)
    {
        var context = new ProkatContext();
        historyRows = new ObservableCollection<HistoryRow>(context.Staff.Include(u => u.UserData).Include(u => u.EntranceStatus)
            .Select(u => new HistoryRow
            {
                loginTime = /*"1"*/ u.LastEntrance.ToString(),
                login = u.UserData.Login,
                loginStatus = /*"1"*/u.EntranceStatus.StatusTittle
            })
            .ToList());
        if (sort == "desc")
        {
            historyRows.OrderBy(h => h.loginTime);
        }
    }

    private void FiltersComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (FiltersComboBox.SelectedIndex == 1)
        {
            SearchTextBox.Text = "По убыванию";
            LoadHistory("desc");
            DataContext = this;
        }

    }
}