
namespace photo_organizer.Models;
public class ProgressInfo
{
    public int TotalFileCount { get; set; } = 0;
    public int MovedFileCount { get; set; } = 0;

    public string CurrentFileName { get; set; } = string.Empty;

    public double ProgressPercentage => (double)MovedFileCount / TotalFileCount * 100;
}