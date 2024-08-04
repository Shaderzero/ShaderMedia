using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Books;

[Index(nameof(Name), IsUnique = true)]
public class Keyword
{
    public int Id { get; set; }
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    public List<BookKeyword> Books { get; set; } = [];
}

public static class KeywordExtensions {
    public static string ToString(this Keyword value)
    {
        return $"Id={value.Id}, name={value.Name}";
    }

    public static int GenerateHash(this Keyword value)
    {
        return int.Abs($"{value.Name.ToLowerInvariant()}".GetHashCode());
    }
}