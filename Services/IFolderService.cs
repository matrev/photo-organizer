using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace photo_organizer.Services;

public interface IFolderService
{
    public Task<IStorageFolder?> PickFolderAsync();
}
