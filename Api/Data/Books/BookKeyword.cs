using Microsoft.EntityFrameworkCore;

namespace Api.Data.Books;

[PrimaryKey(nameof(BookId), nameof(KeywordId))]
public class BookKeyword
{
    public int BookId { get; set; }
    public Book? Book { get; set; }
    public int KeywordId { get; set; }
    public Keyword? Keyword { get; set; }
}