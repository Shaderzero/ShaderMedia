using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Books;

[Index(nameof(Name), IsUnique = true)]
public class Series
{
    public int Id { get; set; }
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;
    public List<Book> Books { get; set; } = [];
}

public static class SerieExtensions {
    public static string ToString(this Series value)
    {
        return $"Id={value.Id}, name={value.Name}";
    }

    public static int GenerateHash(this Series value)
    {
        return int.Abs($"{value.Name.ToLowerInvariant()}".GetHashCode());
    }
}