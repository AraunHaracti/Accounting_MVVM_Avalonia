using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PZ17_MVVM.Models;

namespace PZ17_MVVM.Views.WindowTrainer.Dialogs;

public partial class TrainerAddWindowView : Window
{
    private Action _action;
    
    private Trainer _newTrainer = new();
    
    public TrainerAddWindowView(Action action)
    {
        DataContext = _newTrainer;
        
        InitializeComponent();

        _action += action;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        string sql = $"insert into trainer (FirstName, MiddleName, LastName) " +
                     $"values ('{_newTrainer.FirstName}', " +
                     $"'{_newTrainer.MiddleName}', " +
                     $"'{_newTrainer.LastName}')";

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