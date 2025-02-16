namespace NameSorter.Domain.Interfaces;


/// <summary>
/// Interface for file reading service.
/// </summary>
public interface IFileService
{
    Task<IEnumerable<string>> ReadFileAsync(string filePath);

    Task WriteFileAsync(string filePath, IEnumerable<string> content);
}
