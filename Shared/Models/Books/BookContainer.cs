namespace Shared.Models.Books;

public class BookContainer
{
    public List<BookDto> Books { get; set; } = [];
    public Pagination Pagination { get; set; } = new();
}