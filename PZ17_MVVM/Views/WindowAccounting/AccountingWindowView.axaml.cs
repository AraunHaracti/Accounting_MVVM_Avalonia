using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PZ17_MVVM.ViewModels;

namespace PZ17_MVVM.Views.WindowAccounting;

public partial class AccountingWindowView : Window
{
    public AccountingWindowView()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        DataContext = new AccountingWindowViewModel(this);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void Exit_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}