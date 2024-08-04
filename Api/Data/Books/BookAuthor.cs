using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Books;

[PrimaryKey(nameof(BookId), nameof(AuthorId))]
[Index(nameof(BookId))]
[Index(nameof(AuthorId))]
public class BookAuthor
{
    public int BookId { get; set; }
    public Book? Book { get; set; }
    public int AuthorId { get; set; }
    public Author? Author { get; set; }

    [NotMapped]
    public string AuthorName =>
        Author is null ? string.Empty : $"{Author.LastName} {Author.FirstName} {Author.MiddleName}";
}