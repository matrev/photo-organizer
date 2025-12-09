using System;
using System.IO;
using System.Threading.Tasks;

namespace photo_organizer.Services;

public class PhotoOrganizerService : IPhotoOrganizerService 
{
    public async Task MovePhotosAsync(string sourcePath, string destinationPath)
    {
        await Task.Run(() =>
        {
            var sourceDir = new DirectoryInfo(sourcePath);
            var destDir = new DirectoryInfo(destinationPath);

            if(!sourceDir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory not found: {sourcePath}");
            }
            if(!destDir.Exists)
            {
                destDir.Create();
            }

            foreach (var file in sourceDir.GetFiles())
            {
                // file.MoveTo(Path.Combine(destinationPath, file.Name), overwrite: true);
                // just print the file names and paths for now
                Console.WriteLine($"Moving file: {file.FullName} to {Path.Combine(destinationPath, file.Name)}");

            }
        });
    }
}