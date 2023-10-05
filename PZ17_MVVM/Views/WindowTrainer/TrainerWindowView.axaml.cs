using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PZ17_MVVM.ViewModels;

namespace PZ17_MVVM.Views.WindowTrainer;

public partial class TrainerWindowView : Window
{
    public TrainerWindowView()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        DataContext = new TrainerWindowViewModel(this);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}