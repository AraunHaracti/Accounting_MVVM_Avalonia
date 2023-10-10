using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PZ17_MVVM.Models;

namespace PZ17_MVVM.Views.WindowAccounting.Dialogs;

public partial class AccountingAddWindowView : Window
{
    private Action _action;
    
    private Accounting _newAccounting = new();
    
    public AccountingAddWindowView(Action action)
    {
        DataContext = _newAccounting;
        
        InitializeComponent();

        _action += action;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        string sql = $"insert into accounting (ClientId, TrainerId, CountClasses, StartDate) " +
                     $"values ('{_newAccounting.ClientId}', '{_newAccounting.TrainerId}', '" +
                     $"{_newAccounting.CountClasses}', '{_newAccounting.StartDate.ToString("yyyy-MM-dd H:mm:ss")}')";


        Database.Open();
        try
        {
            Database.SetData(sql);

            _action.Invoke();

            Close();
        }
        catch (Exception ex)
        {
            
        }
        finally
        {
            Database.Exit();
        }
    }

    private void Cancel_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}