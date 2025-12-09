using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace photo_organizer.Services;

public class FolderService : IFolderService
{
    private readonly Window _target;

    public FolderService(Window target)
    {
        _target = target;
    }

    public async Task<IStorageFolder?> PickFolderAsync()
    {
        var folders = await _target.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select a Folder",
            AllowMultiple = false
        });
        return folders.Count > 0 ? folders[0] : null;
    }
}
