using Moq;
using NameSorter.Domain.Interfaces;
using NameSorter.Domain.Models;
using NameSorter.Domain.Services;

namespace NameSorter.Tests;

/// <summary>
/// Unit tests for NameSorter.
/// </summary>
public class SorterServiceTests
{
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly ISorterService _sorterService;

    public SorterServiceTests()
    {
        _fileServiceMock = new Mock<IFileService>();
        _sorterService = new SorterService(_fileServiceMock.Object);
    }

    /// <summary>
    /// Tests that an exception is thrown for invalid name formats.
    /// </summary>
    [Theory]
    [InlineData("John")]
    [InlineData(" Doe")]
    [InlineData("Jane   ")]  // Only one name, should fail
    public void Person_InvalidNameFormat_ThrowsArgumentException(string invalidName)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Person(invalidName));

        // Ensure the exception message is correct
        Assert.Contains("Invalid name format", exception.Message);
    }

    /// <summary>
    /// Tests that an exception is thrown when a person has more than 3 given names.
    /// </summary>
    [Fact]
    public void Person_TooManyGivenNames_ThrowsArgumentException()
    {
        // Act & Assert
        var invalidName = "Ryan 1 2 3 Rodriguez";
        var exception = Assert.Throws<ArgumentException>(() => new Person(invalidName));

        // Ensure the exception message is correct
        Assert.Contains("A person can have at most 3 given names", exception.Message);
    }

    /// <summary>
    /// Tests if last names are correctly captured.
    /// </summary>
    [Fact]
    public void Person_LastNameCapturedCorrectly()
    {
        var person1 = new Person("Ryan Andal Rodriguez");
        var person2 = new Person("Shirley Anore Aran");
        var person3 = new Person("Raja Shamikha Rodriguez");

        Assert.Equal("Rodriguez", person1.LastName);
        Assert.Equal("Aran", person2.LastName);
        Assert.Equal("Rodriguez", person3.LastName);
    }


    /// <summary>
    /// Tests if names are correctly sorted and written to a file.
    /// </summary>
    [Fact]
    public async Task SortAndWriteNamesAsync_WritesSortedNamesCorrectly()
    {
        var testNames = new List<string>
        {
            "Ryan ABC",
            "Ryan HIJ",
            "Ryan AAA",
            "Ryan Zed",
            "Ryan Ryan"
        };

        var expectedOrder = new List<string>
        {
            "Ryan AAA",
            "Ryan ABC",
            "Ryan HIJ",
            "Ryan Ryan",
            "Ryan Zed"
        };

        _fileServiceMock.Setup(fs => fs.ReadFileAsync(It.IsAny<string>())).ReturnsAsync(testNames);
        _fileServiceMock.Setup(fs => fs.WriteFileAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>()))
                        .Returns(Task.CompletedTask)
                        .Verifiable();

        await _sorterService.SortAndWriteNamesAsync("dummyInputPath", "dummyOutputPath");

        _fileServiceMock.Verify(fs => fs.WriteFileAsync("dummyOutputPath", expectedOrder), Times.Once);
    }
}