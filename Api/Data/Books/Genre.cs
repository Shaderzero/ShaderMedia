using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Books;

[Index(nameof(Name), IsUnique = true)]
public class Genre
{
    public int Id { get; set; }
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    public List<BookGenre> Books { get; set; } = [];
}

public static class GenreExtensions {
    public static string ToString(this Genre value)
    {
        return $"Id={value.Id}, name={value.Name}";
    }

    public static int GenerateHash(this Genre value)
    {
        return int.Abs($"{value.Name.ToLowerInvariant()}".GetHashCode());
    }
}