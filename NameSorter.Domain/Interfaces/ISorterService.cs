namespace NameSorter.Domain.Interfaces;

/// <summary>
/// Interface for sorting names.
/// </summary>
public interface ISorterService
{
    Task SortAndWriteNamesAsync(string inputFilePath, string outputFilePath);
}