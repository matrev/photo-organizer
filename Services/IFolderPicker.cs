using System;
using System.Threading.Tasks;

namespace photo_organizer.Services;

public interface IFolderPicker
{
    Task<string?> PickFolderAsync();
}
