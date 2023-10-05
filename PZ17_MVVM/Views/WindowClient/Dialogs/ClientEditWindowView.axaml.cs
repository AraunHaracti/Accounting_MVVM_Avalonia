using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PZ17_MVVM.Models;

namespace PZ17_MVVM.Views.WindowClient.Dialogs;

public partial class ClientEditWindowView : Window
{
    private Action _action;
    
    private Client _resultClient;
    
    public ClientEditWindowView(Action action, Client client)
    {
        _resultClient = client;

        DataContext = _resultClient;
        
        InitializeComponent();

        _action += action;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Edit_OnClick(object? sender, RoutedEventArgs e)
    {
        string sql = $"update pz17.client as c set " +
                     $"c.FirstName = '{_resultClient.FirstName}', " +
                     $"c.MiddleName = '{_resultClient.MiddleName}', " +
                     $"c.LastName = '{_resultClient.LastName}', " +
                     $"c.DOB = '{_resultClient.Dob.ToString("yyyy-MM-dd H:mm:ss")}' " +
                     $"where c.ClientID = '{_resultClient.ClientId}'";
        
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