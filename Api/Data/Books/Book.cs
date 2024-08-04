using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Books;

[Index(nameof(Title))]
public class Book
{
    public int Id { get; set; }
    public List<BookAuthor> Authors { get; set; } = [];
    public List<BookGenre> Genres { get; set; } = [];
    [MaxLength(512)]
    public string Title { get; set; } = string.Empty;
    public int? SeriesId { get; set; }
    public Series? Series { get; set; }
    public int? SeriesNo { get; set; }
    public int? LibRate { get; set; }
    [MaxLength(255)]
    public string ZipName { get; set; } = string.Empty;
    public int File { get; set; }
    public int Size { get; set; }
    public bool Del { get; set; }
    [MaxLength(4)]
    public string? Ext { get; set; }
    public DateOnly? Date { get; set; }
    public int? LanguageId { get; set; }
    public Language? Language { get; set; }
    public List<BookKeyword> Keywords { get; set; } = [];
}