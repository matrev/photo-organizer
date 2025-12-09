using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.Input;

namespace photo_organizer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";

    [RelayCommand]
    private async Task OpenFile()
    {
        // File picker implementation can go here
    }
}
