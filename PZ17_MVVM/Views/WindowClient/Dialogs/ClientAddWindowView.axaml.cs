using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PZ17_MVVM.Models;

namespace PZ17_MVVM.Views.WindowClient.Dialogs;

public partial class ClientAddWindowView : Window
{
    private Action _action;
    
    private Client _newClient = new();
    
    public ClientAddWindowView(Action action)
    {
        DataContext = _newClient;
        
        InitializeComponent();

        _action += action;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        string sql = $"insert into client (FirstName, MiddleName, LastName, DOB) " +
                     $"values ('{_newClient.FirstName}', '{_newClient.MiddleName}', '" +
                     $"{_newClient.LastName}', '{_newClient.Dob.ToString("yyyy-MM-dd H:mm:ss")}')";

        Database.Open();
        Database.SetData(sql);
        Database.Exit();
        
        _action.Invoke();
        
        Close();
    }

    private void Cancel_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}