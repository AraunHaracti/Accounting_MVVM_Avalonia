using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PZ17_MVVM.Models;

namespace PZ17_MVVM.Views.WindowAccounting.Dialogs;

public partial class AccountingEditWindowView : Window
{
    private Action _action;
    
    private Accounting _resultAccounting;
    
    public AccountingEditWindowView(Action action, Accounting accounting)
    {
        _resultAccounting = accounting;

        DataContext = _resultAccounting;
        
        InitializeComponent();

        _action += action;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Edit_OnClick(object? sender, RoutedEventArgs e)
    {
        string sql = $"update pz17.accounting as a set " +
                     $"a.ClientId = '{_resultAccounting.ClientId}', " +
                     $"a.TrainerId = '{_resultAccounting.TrainerId}', " +
                     $"a.CountClasses = '{_resultAccounting.CountClasses}', " +
                     $"a.StartDate = '{_resultAccounting.StartDate.ToString("yyyy-MM-dd H:mm:ss")}' " +
                     $"where a.AccountingID = '{_resultAccounting.AccountingId}'";

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