using System.Threading.Tasks;
using photo_organizer.Services;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace photo_organizer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IPhotoOrganizerService _photoOrganizerService;
    private readonly IFolderService _folderService;
    public string Greeting { get; } = "Welcome to Avalonia!";

    [ObservableProperty]
    private string? _sourceFolderPath;

    [ObservableProperty]
    private string? _destinationFolderPath;

    public MainWindowViewModel(IPhotoOrganizerService photoOrganizerService, IFolderService folderService)
    {
        _photoOrganizerService = photoOrganizerService;
        _folderService = folderService;
    }


    [RelayCommand]
    private async Task SelectFolder(string folderType)
    {

        // File picker implementation can go here
        var folder = await _folderService.PickFolderAsync();

        if (folder == null) return;

        var path = folder.Path.LocalPath;
        if(folderType == "source")
        {
            SourceFolderPath = path;
        }
        else if(folderType == "destination")
        {
            DestinationFolderPath = path;
        }
        Console.WriteLine($"Picked folder: {folder.Path.LocalPath}");
    }

    [RelayCommand]
    private async Task OrganizePhotos()
    {
        Console.WriteLine("Organizing photos...");
        try
        {
            if(string.IsNullOrEmpty(SourceFolderPath) || string.IsNullOrEmpty(DestinationFolderPath))
            {
                Console.WriteLine("Source or Destination folder path is not set.");
                return;
            }

            await _photoOrganizerService.MovePhotosAsync(SourceFolderPath, DestinationFolderPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error organizing photos: {ex.Message}");
            throw;
        }
    }
}
