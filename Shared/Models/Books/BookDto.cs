namespace Shared.Models.Books;

public class BookDto
{
    public int Id { get; set; }
    public List<AuthorDto> Authors { get; set; } = [];
    public List<GenreDto> Genres { get; set; } = [];
    public string Title { get; set; } = string.Empty;
    public SeriesDto? Series { get; set; }
    public int? SeriesNo { get; set; }
    public int? LibRate { get; set; }
    public string ZipName { get; set; } = string.Empty;
    public int File { get; set; }
    public int Size { get; set; }
    public bool Del { get; set; }
    public string? Ext { get; set; }
    public DateOnly? Date { get; set; }
    public LanguageDto? Language { get; set; }
    public List<KeywordDto>? Keywords { get; set; }
    public string? Annotation { get; set; }
    public BookCover? Cover { get; set; }
}