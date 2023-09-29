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
}