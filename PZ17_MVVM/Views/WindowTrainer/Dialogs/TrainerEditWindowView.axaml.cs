using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PZ17_MVVM.Models;

namespace PZ17_MVVM.Views.WindowTrainer.Dialogs;

public partial class TrainerEditWindowView : Window
{
    private Action _action;
    
    private Trainer _resultTrainer;
    
    public TrainerEditWindowView(Action action, Trainer trainer)
    {
        _resultTrainer = trainer;

        DataContext = _resultTrainer;
        
        InitializeComponent();

        _action += action;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    private void Edit_OnClick(object? sender, RoutedEventArgs e)
    {
        string sql = $"update pz17.trainer as t set " +
                     $"t.FirstName = '{_resultTrainer.FirstName}', " +
                     $"t.MiddleName = '{_resultTrainer.MiddleName}', " +
                     $"t.LastName = '{_resultTrainer.LastName}' " +
                     $"where t.TrainerID = '{_resultTrainer.TrainerId}'";
        
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