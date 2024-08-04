namespace Shared.Models.Books;

public class GenreDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public static class GenreExtensions
{
    public static IEnumerable<string> GenerateName(this IEnumerable<GenreDto> genres)
    {
        return genres.Select(genre => genre.Name);
    }
}