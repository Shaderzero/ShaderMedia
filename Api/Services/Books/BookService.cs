using System.IO.Compression;
using Api.Repositories.Interfaces.Books;
using Api.Services.Books.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shared.Dictionaries;
using Shared.Models;
using Shared.Models.Books;

namespace Api.Services.Books;

public class BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper, ILogger<BookService> logger) : IBookService
{
    public async Task<BookContainer> GetBooksAsync(BooksRequest request)
    {
        var pagination = request.Pagination;
        var pageSize = pagination.PageSize;
        var pageNumber = pagination.PageNumber;
        var books = bookRepository.GetAll();

        if (!string.IsNullOrEmpty(request.SearchText))
        {
            var str = request.SearchText.ToLower();

            books = books.Where(x => request.SearchTitle && x.Title.ToLower().Contains(str)
                                     || request.SearchAuthor && x.Authors.Any(a =>
                                         a.Author != null && a.Author.LastName != null &&
                                         a.Author.LastName.ToLower().Contains(str))
                                     || request.SearchGenre && x.Genres.Any(g =>
                                         g.Genre != null && g.Genre.Name.ToLower().Contains(str))
                                     || request.SearchKeyword && x.Keywords.Any(k =>
                                         k.Keyword != null && k.Keyword.Name.ToLower().Contains(str))
                                     || request.SearchSeries && x.Series != null &&
                                     x.Series.Name.ToLower().Contains(str));
        }

        books = request.SortColumn switch
        {
            BookSortColumn.Author => request.SortDirection == SortDirection.Up
                ? books.OrderByDescending(x => x.Authors.Count != 0 ? x.Authors.First().Author!.LastName : string.Empty)
                : books.OrderBy(x => x.Authors.Count != 0 ? x.Authors.First().Author!.LastName : string.Empty),
            BookSortColumn.Title => request.SortDirection == SortDirection.Up 
                ? books.OrderByDescending(x => x.Title) 
                : books.OrderBy(x => x.Title),
            BookSortColumn.Series => request.SortDirection == SortDirection.Up
                ? books.OrderByDescending(x => x.Series != null ? x.Series.Name : string.Empty)
                : books.OrderBy(x => x.Series != null ? x.Series.Name : string.Empty),
            BookSortColumn.Genre => request.SortDirection == SortDirection.Up
                ? books.OrderByDescending(x => x.Genres.Count != 0 ? x.Genres.First().Genre!.Name : string.Empty)
                : books.OrderBy(x => x.Genres.Count != 0 ? x.Genres.First().Genre!.Name : string.Empty),
            BookSortColumn.Data => request.SortDirection == SortDirection.Up
                ? books.OrderByDescending(x => x.Date)
                : books.OrderBy(x => x.Date),
            _ => request.SortDirection == SortDirection.Up 
                ? books.OrderByDescending(x => x.Title) 
                : books.OrderBy(x => x.Title),
        };

        var count = await books.CountAsync();
        if (count < (pageNumber - 1) * pageSize)
            pageNumber = 1;
        books = books.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        var result = await books.ToListAsync();
        var bookList = mapper.Map<List<BookDto>>(result);
        return new BookContainer
        {
            Books = bookList,
            Pagination = new Pagination
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
                Count = count
            }
        };
    }

    public async Task<BookDto?> GetBookByIdAsync(int bookId)
    {
        var bookDb = await bookRepository.GetByIdAsync(bookId);
        if (bookDb is null)
            return null;
        
        var book = mapper.Map<BookDto>(bookDb);
        return book;
    }
}