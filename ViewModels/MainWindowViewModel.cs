using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using photo_organizer.Models;
using photo_organizer.Services;

namespace photo_organizer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IFolderService _folderService;
    private readonly IPhotoOrganizerService _photoOrganizerService;

    [ObservableProperty]
    private string? _sourceFolderPath;

    [ObservableProperty]
    private string? _destinationFolderPath;

    [ObservableProperty]
    private bool _isOrganizing;

    [ObservableProperty]
    private string? _statusMessage;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private string? _fileCountDisplay;

    [ObservableProperty]
    private double _progressPercentage;


    [ObservableProperty]
    private string? _currentFileName;


    public MainWindowViewModel(IFolderService folderService, IPhotoOrganizerService photoOrganizerService)
    {
        _folderService = folderService;
        _photoOrganizerService = photoOrganizerService;
    }

    [RelayCommand]
    private async Task SelectFolder(string folderType)
    {
        ResetMessages();

        var folder = await _folderService.PickFolderAsync();
        if (folder == null)
            return;

        var path = folder.Path.LocalPath;

        if (folderType == "source")
        {
            if (await _folderService.IsFolderEmpty(folder))
            {
                ErrorMessage = "Selected source folder is empty. Please select a folder with content.";
                return;
            }

            SourceFolderPath = path;
        }
        else if (folderType == "destination")
        {
            DestinationFolderPath = path;
        }
    }

    [RelayCommand]
    private async Task OrganizePhotos()
    {
        ResetMessages();
        var progress = new Progress<ProgressInfo>(OnProgressChanged);

        if (!ValidateFolderPaths())
            return;

        try
        {
            StatusMessage = "Organizing photos...";
            IsOrganizing = true;

            var photosMoved = await _photoOrganizerService.MovePhotosAsync(SourceFolderPath!, DestinationFolderPath!, progress);

            StatusMessage = $"Photos organized successfully! {photosMoved} file(s) moved from {SourceFolderPath} to {DestinationFolderPath}.";
            ClearFolderPaths();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error organizing photos: {ex.Message}";
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            IsOrganizing = false;
        }
    }

    private void OnProgressChanged(ProgressInfo info)
    {
        FileCountDisplay = $"{info.MovedFileCount} / {info.TotalFileCount} files moved";
        CurrentFileName = info.CurrentFileName;
        ProgressPercentage = info.ProgressPercentage;
    }

    private bool ValidateFolderPaths()
    {
        if (string.IsNullOrEmpty(SourceFolderPath) || string.IsNullOrEmpty(DestinationFolderPath))
        {
            ErrorMessage = "Both source and destination folder paths must be set.";
            return false;
        }

        if (SourceFolderPath == DestinationFolderPath)
        {
            ErrorMessage = "Source and destination folders cannot be the same.";
            return false;
        }

        return true;
    }

    private void ClearFolderPaths()
    {
        SourceFolderPath = null;
        DestinationFolderPath = null;
    }

    private void ResetMessages()
    {
        ErrorMessage = null;
        StatusMessage = null;
    }
}
