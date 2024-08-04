using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Books;

[Index(nameof(Name), IsUnique = true)]
public class Language
{
    public int Id { get; set; }
    [MaxLength(2)]
    public string Name { get; set; } = string.Empty;
    public List<Book> Books { get; set; } = [];
}

public static class LanguageExtensions {
    public static string ToString(this Language value)
    {
        return $"Id={value.Id}, name={value.Name}";
    }

    public static int GenerateHash(this Language value)
    {
        return int.Abs($"{value.Name.ToLowerInvariant()}".GetHashCode());
    }
}