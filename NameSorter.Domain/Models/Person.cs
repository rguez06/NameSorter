namespace NameSorter.Domain.Models;

/// <summary>
/// Represents a person with given names and a last name.
/// </summary>
public class Person : IComparable
{
    public string LastName { get; set; }
    public List<string> GivenNames { get; set; }

    public Person(string fullName)
    {
        // Split the full name into parts based on space
        var nameParts = fullName.Split(' ').Where(part => !string.IsNullOrEmpty(part)).ToList();

        // Assume that the last part is the last name and others are given names
        LastName = nameParts.Last(); // Last element is the last name
        GivenNames = nameParts.Take(nameParts.Count - 1).ToList(); // The rest are given names
    }

    public override string ToString()
    {
        return $"{string.Join(" ", GivenNames)} {LastName}";
    }

    public int CompareTo(object? obj)
    {
        throw new NotImplementedException();
    }
}
