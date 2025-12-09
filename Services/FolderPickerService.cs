using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace photo_organizer.Services;

public class FolderPickerService : IFolderPicker
{
    private readonly TopLevel _topLevel;

    public FolderPickerService(TopLevel topLevel)
    {
        _topLevel = topLevel;
    }

    public async Task<string?> PickFolderAsync()
    {
        var folders = await _topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select a Folder",
            AllowMultiple = false
        });

        return folders.Count > 0 ? folders[0].Path.LocalPath : null;
    }
}
