using NameSorter.Domain.Interfaces;
using NameSorter.Domain.Models;

namespace NameSorter.Domain.Services;

/// <summary>
/// Name sorter implementation that reads and sorts names from a file.
/// </summary>
public class SorterService : ISorterService
{
    private readonly IFileService _fileService;

    public SorterService(IFileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// Reads names from a file, sorts them by last name, then by given names, and writes the output to a file.
    /// </summary>
    public async Task SortAndWriteNamesAsync(string inputFilePath, string outputFilePath)
    {
        var names = await _fileService.ReadFileAsync(inputFilePath);
        var sortedNames = names.AsParallel()
                               .Select(name => new Person(name))
                               .OrderBy(p => (p.LastName, p.GivenNames))
                               .Select(p => p.ToString())
                               .ToList();

        foreach (var name in sortedNames)
        {
            Console.WriteLine(name);
        }

        await _fileService.WriteFileAsync(outputFilePath, sortedNames);
    }
}