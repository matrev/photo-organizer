using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using photo_organizer.Models;

namespace photo_organizer.Services;

public class PhotoOrganizerService : IPhotoOrganizerService 
{

    //to avoid creating same directory multiple times in concurrent environment
    private readonly ConcurrentDictionary<string, byte> _createdDirectories = new();

    private readonly HashSet<string> _photoExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".heic", ".webp", ".raw"
    };
    
    public async Task<int> MovePhotosAsync(string sourcePath, string rootDestinationPath, IProgress<ProgressInfo> progress)
    {
        var sourceDir = new DirectoryInfo(sourcePath);
        var destDir = new DirectoryInfo(rootDestinationPath);

        if (!sourceDir.Exists) {
            throw new DirectoryNotFoundException($"Source directory not found: {sourcePath}");
        }
        
        if (!destDir.Exists) {
            destDir.Create();
        }

        List<FileInfo> photoFiles = sourceDir.GetFiles()
            .Where(f => IsPhotoFile(f.Extension))
            .ToList();

        int totalFiles = photoFiles.Count;
        int completedFiles = 0;
        object lockObject = new object();

        const int MaxConcurrentMoves = 8;
        using var semaphore = new SemaphoreSlim(MaxConcurrentMoves, MaxConcurrentMoves);



        IEnumerable<Task<bool>> tasks = photoFiles.Select(async file => 
        {
            await semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                bool ok = await MoveFileAsync(file, rootDestinationPath, () =>
                {
                    lock (lockObject)
                    {
                        int finished = Interlocked.Increment(ref completedFiles);
                        progress?.Report(new ProgressInfo
                        {
                            TotalFileCount = totalFiles,
                            MovedFileCount = finished,
                            CurrentFileName = file.Name
                        });
                    }
                
                });
                return ok;
            } finally
            {
                semaphore.Release();
            }
            
        });

        bool[] results = await Task.WhenAll(tasks);

        return results.Count(success => success);
    }

    private async Task<bool> MoveFileAsync(FileInfo file, string destinationRootPath, Action onCompleted) {
        //read dateTime from metadata
        IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(file.FullName);

        // try to get DateTimeOriginal from EXIF
        var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
        DateTime dateTime = GetDateTaken(file.FullName);

        //extract year and month
        string year = dateTime.Year.ToString();
        string month = dateTime.Month.ToString("D2");
        string targetFolder = Path.Combine(destinationRootPath, year, month);

        //move to year/month folder
        _createdDirectories.GetOrAdd(targetFolder, _ => {
            System.IO.Directory.CreateDirectory(targetFolder);
            return 0;
        });

        return await Task.Run(() => {
            try {
                file.CopyTo(Path.Combine(targetFolder, file.Name), overwrite: true);
                Console.WriteLine($"Moved file: {file.Name} | Date Taken: {dateTime} | Year: {year} | Month: {month}");
                return true;
            } catch (Exception ex) {
                Console.WriteLine($"Failed to move file: {file.Name}. Error: {ex.Message}");
                return false;
            } finally {
                onCompleted?.Invoke();
            }
        }).ConfigureAwait(false);
    }

    //process date taken from metadata, fallback to file creation date
    private DateTime GetDateTaken(string filePath)
    {
        try
        {
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(filePath);
            var subIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

            if (subIfdDirectory != null && subIfdDirectory.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out DateTime dateTime))
            {
                return dateTime;
            }
        }
        catch (ImageProcessingException)
        {
            // Handle cases where metadata cannot be read
        }

        return File.GetCreationTime(filePath);
    }

    private bool IsPhotoFile(string fileExtension)
    {
        return _photoExtensions.Contains(fileExtension);
    }
}