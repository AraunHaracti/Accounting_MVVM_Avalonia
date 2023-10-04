using System;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PZ17_MVVM.Views.WindowAccounting;
using PZ17_MVVM.Views.WindowClient;
using PZ17_MVVM.Views.WindowTrainer;

namespace PZ17_MVVM.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void OnClickDisplayClients(object? sender, RoutedEventArgs e)
    {
        ClientWindowView window = new ClientWindowView();
        window.ShowDialog(this);
    }

    public void OnClickDisplayTrainer(object? sender, RoutedEventArgs e)
    {
        TrainerWindowView window = new TrainerWindowView();
        window.ShowDialog(this);
    }

    public void OnClickDisplayAccounting(object? sender, RoutedEventArgs e)
    {
        AccountingWindowView window = new AccountingWindowView();
        window.ShowDialog(this);
    }

    public void OnClockAddDataToClientsTable(object? sender, RoutedEventArgs e)
    {
        Database.Open();


        for (int i = 0; i < 1000; i++)
        {
            string sql = $"insert into client (FirstName, MiddleName, LastName, DOB) " +
                         $"values ('{GenerationString(10)}', '{GenerationString(10)}', '{GenerationString(10)}', '{DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")}')";
        
            Database.SetData(sql);

        }
        
        Database.Exit();
    }

    private string GenerationString(int count)
    {
        Random random = new Random();

        StringBuilder resultString = new StringBuilder();
        
        for (int i = 0; i < count; i++)
        {
            int randomNumber = random.Next(26);
            resultString.Append((char)(randomNumber + 97));
        }

        return resultString.ToString();
    }
}