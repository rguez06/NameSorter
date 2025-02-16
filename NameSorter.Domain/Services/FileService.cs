using NameSorter.Domain.Interfaces;

namespace NameSorter.Domain.Services;

/// <summary>
/// Service responsible for reading names from a file asynchronously.
/// </summary>
public class FileService : IFileService
{
    public async Task<IEnumerable<string>> ReadFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found.", filePath);
        }
        return await File.ReadAllLinesAsync(filePath);
    }

    public async Task WriteFileAsync(string filePath, IEnumerable<string> content)
    {
        await File.WriteAllLinesAsync(filePath, content);
    }
}
