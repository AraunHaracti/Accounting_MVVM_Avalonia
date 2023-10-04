using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PZ17_MVVM.ViewModels;

namespace PZ17_MVVM.Views.WindowClient;

public partial class ClientWindowView : Window
{
    public ClientWindowView()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        DataContext = new ClientWindowViewModel(this);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}