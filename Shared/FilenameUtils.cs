using Shared.Models.Books;

namespace Shared;

public static class FilenameUtils
{
    public static string GetBookSafeName(BookDto book)
    {
        var result = book.Title!;
        if (book.Authors.Count > 0)
        {
            var firstAuthor = book.Authors.FirstOrDefault();
            if (firstAuthor != default)
            {
                result = $"{firstAuthor.GenerateName()} - {result}";
            }
        }
        return $"{FilterDangerChars(result)}.{book.Ext}";
    }
    
    private static string FilterDangerChars(string s)
    {
        var res = "";
        foreach (var c in s)
        {
            if (DangerChars.TryGetValue(c, out var value)) res += value;
            else res += c;
        }
        return res;
    }
    
    private static readonly Dictionary<char, string> DangerChars = new()
    {
        { '/', "" },
        { '\\', "" },
        { ':', "" },
        { '*', "" },
        { '?', "" },
        { '"', "'" },
        { '<', "«" },
        { '>', "»" },
        { '|', "" },
    };
}