using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Books;

[Index(nameof(LastName), nameof(FirstName), nameof(MiddleName), IsUnique = true)]
[Index(nameof(LastName))]
public class Author
{
    public int Id { get; set; }
    [MaxLength(255)]
    public string? FirstName { get; set; }
    [MaxLength(255)]
    public string? MiddleName { get; set; }
    [MaxLength(255)]
    public string? LastName { get; set; }
    public List<BookAuthor> Books { get; set; } = [];

}

public static class AuthorExtensions {
    public static string ToString(this Author value)
    {
        return $"Id={value.Id}, name={value.LastName} {value.FirstName} {value.MiddleName}";
    }

    public static int GenerateHash(this Author value)
    {
        return int.Abs($"{value.LastName?.ToLowerInvariant()}_{value.FirstName?.ToLowerInvariant()}_{value.MiddleName?.ToLowerInvariant()}".GetHashCode());
    }
}