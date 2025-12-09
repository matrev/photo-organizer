using System.Threading.Tasks;

namespace photo_organizer.Services;

public interface IPhotoOrganizerService
{
    Task MovePhotosAsync(string sourcePath, string destinationPath);
}