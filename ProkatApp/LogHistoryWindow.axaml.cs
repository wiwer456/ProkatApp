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
        
        /*historyRows = new ObservableCollection<HistoryRow>(context.LoginHistories.Include(u => u.UserData).Include(u => u.EntranceStatus)
            .Select(u => new HistoryRow
            {
                loginTime = u.LoginTime.ToString(),
                login = u.UserData.Login,
                loginStatus = u.EntranceStatus.StatusTittle
            })
            .ToList());*/
        if (FiltersComboBox.SelectedItem.ToString() == "По возрастанию")
        {
            historyRows = new ObservableCollection<HistoryRow>(context.LoginHistories.Include(u => u.UserData).Include(u => u.EntranceStatus)
            .Select(u => new HistoryRow
            {
                loginTime = u.LoginTime.ToString(),
                login = u.UserData.Login,
                loginStatus = u.EntranceStatus.StatusTittle
            })
            .ToList()
            .OrderByDescending(h => h.loginTime));
        }
        else
        {
            historyRows = new ObservableCollection<HistoryRow>(context.LoginHistories.Include(u => u.UserData).Include(u => u.EntranceStatus)
            .Select(u => new HistoryRow
            {
                loginTime = u.LoginTime.ToString(),
                login = u.UserData.Login,
                loginStatus = u.EntranceStatus.StatusTittle
            })
            .ToList()
            .OrderBy(h => h.loginTime));
        }
    }

    /*public void ApplyFilters()
    {
        var temp = historyRows.AsQueryable();

        

        historyRows.Clear();
        foreach(var t in temp)
        {
            historyRows.Add(t);
        }
    }*/

    private void FiltersComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (FiltersComboBox.SelectedIndex == 1)
        {
            LoadHistory("desc");
        }
        if (FiltersComboBox.SelectedIndex == 0)
        {
            LoadHistory();
        }
        HistoryListBox.ItemsSource = historyRows;
    }
}