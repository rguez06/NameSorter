using Microsoft.Extensions.DependencyInjection;
using NameSorter.Domain.Interfaces;
using NameSorter.Domain.Services;

namespace NameSorter;

/// <summary>
/// Entry point for the application.
/// </summary>
class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide the file path containing the list of names.");
            return;
        }

        string inputFilePath = args[0];
        string outputFilePath = Path.Combine(AppContext.BaseDirectory, "sorted-names-list.txt");

        var serviceProvider = new ServiceCollection()
            .AddSingleton<IFileService, FileService>()
            .AddSingleton<ISorterService, SorterService>()
            .BuildServiceProvider();

        var nameSorter = serviceProvider.GetRequiredService<ISorterService>();

        if (nameSorter != null)
        {
            try
            {
                await nameSorter.SortAndWriteNamesAsync(inputFilePath, outputFilePath);
                Console.WriteLine($"Sorted names have been written to: {outputFilePath}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
