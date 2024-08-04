using Microsoft.EntityFrameworkCore;

namespace Api.Data.Books;

[PrimaryKey(nameof(BookId), nameof(GenreId))]
public class BookGenre
{
    public int BookId { get; set; }
    public Book? Book { get; set; }
    public int GenreId { get; set; }
    public Genre? Genre { get; set; }
}