namespace NameSorter.Domain.Models;

/// <summary>
/// Represents a person with given names and a last name.
/// </summary>
public record Person
{
    public string LastName { get; }
    public string GivenNames { get; }

    public Person(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Name cannot be empty or null.", nameof(fullName));
        }

        var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            throw new ArgumentException($"Invalid name format: {fullName}. A name must have at least a last name and one given name.");
        }

        if (parts.Length > 4) // Last name + max 3 given names
        {
            throw new ArgumentException($"Invalid name format: {fullName}. A person can have at most 3 given names.");
        }

        LastName = parts[^1] ?? string.Empty;
        GivenNames = string.Join(" ", parts[..^1]) ?? string.Empty;
    }

    public override string ToString() => $"{GivenNames} {LastName}";
}

