using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySqlConnector;
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

    
    
    Random random = new Random();
    
    public void OnClockAddDataToClientsTable(object? sender, RoutedEventArgs e)
    {
        Database.Open();

        for (int i = 0; i < 1000; i++)
        {
            string sql = $"insert into client (FirstName, MiddleName, LastName, DOB) " +
                         $"values ('{GenerationString(10)}', " +
                         $"'{GenerationString(10)}', " +
                         $"'{GenerationString(10)}', " +
                         $"'{DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")}')";
        
            Database.SetData(sql);

        }
        
        Database.Exit();
    }
    
    public void OnClockAddDataToTrainersTable(object? sender, RoutedEventArgs e)
    {
        Database.Open();

        for (int i = 0; i < 1000; i++)
        {
            string sql = $"insert into trainer (FirstName, MiddleName, LastName) " +
                         $"values ('{GenerationString(10)}', " +
                         $"'{GenerationString(10)}', " +
                         $"'{GenerationString(10)}')";
        
            Database.SetData(sql);

        }
        
        Database.Exit();
    }
    
    public void OnClockAddDataToAccountingTable(object? sender, RoutedEventArgs e)
    {
        List<int> clientIds = new List<int>();
        List<int> trainerIds = new List<int>();

        Database.Open();

        MySqlDataReader reader;
        reader = Database.GetData("select ClientID from client");

        while (reader.Read() && reader.HasRows)
        {
            clientIds.Add(reader.GetInt32("ClientId"));
        }
        
        Database.Exit();

        Database.Open();
        
        reader = Database.GetData("select TrainerID from trainer");

        while (reader.Read() && reader.HasRows)
        {
            trainerIds.Add(reader.GetInt32("TrainerId"));
        }
                
        Database.Exit();

        Database.Open();

        for (int i = 0; i < 1000; i++)
        {
            string sql = $"insert into accunting (ClientId, TrainerId, CountClasses, StartDate) " +
                         $"values ({clientIds[random.Next(clientIds.Count - 1)]}, " +
                         $"{trainerIds[random.Next(trainerIds.Count - 1)]}, " +
                         $"{random.Next(1000)}, " +
                         $"'{DateTime.Now.ToString("yyyy-MM-dd H:mm:ss")}')";
            Database.SetData(sql);
        }
        
        Database.Exit();
    }

    private string GenerationString(int count)
    {
        StringBuilder resultString = new StringBuilder();
        
        for (int i = 0; i < count; i++)
        {
            int randomNumber = random.Next(26);
            resultString.Append((char)(randomNumber + 97));
        }

        return resultString.ToString();
    }
}