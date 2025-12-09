using System;
using System.Threading.Tasks;
using photo_organizer.Models;

namespace photo_organizer.Services;

public interface IPhotoOrganizerService
{
    Task<int> MovePhotosAsync(string sourcePath, string destinationPath, IProgress<ProgressInfo> progress);
}