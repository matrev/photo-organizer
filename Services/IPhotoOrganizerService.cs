using System.Threading.Tasks;

namespace photo_organizer.Services;

public interface IPhotoOrganizerService
{
    Task<int> MovePhotosAsync(string sourcePath, string destinationPath);
}