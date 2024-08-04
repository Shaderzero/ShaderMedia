using Shared.Dictionaries;

namespace Shared.Models.Books;

public class BooksRequest
{
    public Pagination Pagination { get; set; }
    public BookSortColumn SortColumn { get; set; }
    public SortDirection SortDirection { get; set; }
    public string? SearchText { get; set; }
    public bool SearchAuthor { get; set; }
    public bool SearchTitle { get; set; }
    public bool SearchGenre { get; set; }
    public bool SearchSeries { get; set; }
    public bool SearchKeyword { get; set; }

    public BooksRequest()
    {
        Pagination = new Pagination
        {
            PageNumber = 1,
            PageSize = 10,
        };
        SortColumn = BookSortColumn.Title;
        SortDirection = SortDirection.Down;
        SearchAuthor = true;
        SearchTitle = true;
        SearchGenre = true;
        SearchSeries = true;
        SearchKeyword = true;
    }
}