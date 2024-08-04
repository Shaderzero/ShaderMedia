using System.Text;

namespace Shared.Models.Books;

public class AuthorDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

public static class AuthorExtensions
{
    public static string GenerateName(this AuthorDto author)
    {
        var sb = new StringBuilder();
        if (!string.IsNullOrEmpty(author.LastName))
        {
            sb.Append(author.LastName);
        }

        if (!string.IsNullOrEmpty(author.FirstName))
        {
            if (sb.Length > 0)
                sb.Append(' ');
            sb.Append(author.FirstName);
        }

        if (!string.IsNullOrEmpty(author.MiddleName))
        {
            if (sb.Length > 0)
                sb.Append(' ');
            sb.Append(author.MiddleName);
        }

        return sb.ToString();
    }

    public static IEnumerable<string> GenerateName(this IEnumerable<AuthorDto> authors)
    {
        return authors.Select(author => author.GenerateName());
    }
}