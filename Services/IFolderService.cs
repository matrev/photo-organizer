using System;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace photo_organizer.Services;

public interface IFolderService
{
    public Task<IStorageFolder?> PickFolderAsync();

    public Task<bool> IsFolderEmpty(IStorageFolder folder);
}
