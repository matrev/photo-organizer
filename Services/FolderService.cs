using System;
using System.Collections.Generic;
using System.Linq;
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
        //do not allow folder picker to select empty folders
        var folders = await _target.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select a Folder",
            AllowMultiple = false,

        });
        return folders.Count > 0 ? folders[0] : null;
    }

    public async Task<bool> IsFolderEmpty(IStorageFolder folder)
    {
        var items = await folder.GetItemsAsync().ToListAsync();
        return items.Count == 0;
    }
}
