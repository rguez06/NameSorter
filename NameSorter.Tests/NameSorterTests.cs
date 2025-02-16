using Moq;
using NameSorter.Domain.Interfaces;
using NameSorter.Domain.Models;
using NameSorter.Domain.Services;

namespace NameSorter.Tests;

/// <summary>
/// Unit tests for NameSorter.
/// </summary>
public class NameSorterTests
{
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly ISorterService _sorterService;

    public NameSorterTests()
    {
        _fileServiceMock = new Mock<IFileService>();
        _sorterService = new SorterService(_fileServiceMock.Object);
    }

    /// <summary>
    /// Tests if names are correctly sorted and written to a file.
    /// </summary>
    [Fact]
    public async Task SortAndWriteNamesAsync_WritesSortedNamesCorrectly()
    {
        var testNames = new List<string>
        {
            "John Michael Doe",
            "Alice Johnson",
            "Michael Andrew Smith",
            "John A. Doe",
            "Zara Anne Lee",
            "Michael B. Smith"
        };

        var expectedOrder = new List<string>
        {
            "John A. Doe",
            "John Michael Doe",
            "Alice Johnson",
            "Zara Anne Lee",
            "Michael Andrew Smith",
            "Michael B. Smith"
        };

        _fileServiceMock.Setup(fs => fs.ReadFileAsync(It.IsAny<string>())).ReturnsAsync(testNames);
        _fileServiceMock.Setup(fs => fs.WriteFileAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                        .Returns(Task.CompletedTask)
                        .Verifiable();

        await _sorterService.SortAndWriteNamesAsync("dummyInputPath", "dummyOutputPath");

        _fileServiceMock.Verify(fs => fs.WriteFileAsync("dummyOutputPath", expectedOrder), Times.Once);
    }

    /// <summary>
    /// Tests if last names are correctly captured.
    /// </summary>
    [Fact]
    public void Person_LastNameCapturedCorrectly()
    {
        var person1 = new Person("John Michael Doe");
        var person2 = new Person("Alice Johnson");
        var person3 = new Person("Michael Andrew Smith");

        Assert.Equal("Doe", person1.LastName);
        Assert.Equal("Johnson", person2.LastName);
        Assert.Equal("Smith", person3.LastName);
    }
}