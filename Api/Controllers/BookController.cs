using System.IO.Compression;
using Api.Helpers;
using Api.Services.Books.Interfaces;
using Api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared;
using Shared.Models;
using Shared.Models.Books;
using Shared.Utils;

namespace Api.Controllers;

[ApiController]
public class BookController(IBookService bookService, MimeHelper mimeHelper, IOptionsMonitor<ApplicationSettings> options, ILogger<BookController> logger) : ControllerBase
{
    // private const string LibraryPath = @"S:\Books\fb2.Flibusta.Net";
    private readonly string _libraryPath = options.CurrentValue.BooksPath;
    
    [HttpPost]
    [Route(ApiPaths.Books)]
    public async Task<ApiResponse<BookContainer>> GetBooksAsync([FromBody] BooksRequest request)
    {
        var bookContainer = await bookService.GetBooksAsync(request);
        return new ApiResponse<BookContainer> { Value = bookContainer };
    }
    
    [HttpGet]
    [Route(ApiPaths.BookDownload)]
    public async Task<IActionResult> GetBookAsync([FromRoute] int bookId)
    {
        var book = await bookService.GetBookByIdAsync(bookId);
        if (book is null)
            return NotFound();
        
        var archive = Path.Combine(_libraryPath, book.ZipName);
        var filename = $"{book.File}.{book.Ext}";
        
        var stream = GetBookFileFromArchive(archive, filename);
        
        return File(stream, mimeHelper.GetContentType(book.Ext!), FilenameUtils.GetBookSafeName(book));
    }

    [HttpGet]
    [Route(ApiPaths.BookInfo)]
    public async Task<IActionResult> GetBookInfoAsync([FromRoute] int bookId)
    {
        var book = await bookService.GetBookByIdAsync(bookId);
        if (book is null)
            return NotFound();
        
        var archive = Path.Combine(_libraryPath, book.ZipName);
        var filename = $"{book.File}.{book.Ext}";
        
        var stream = GetBookFileFromArchive(archive, filename);

        if (await Fb2Service.ReadAsync(book, stream, HttpContext.RequestAborted))
        {
            return Ok(book);
        }
        
        return NotFound();
    }
    
    private Stream GetBookFileFromArchive(string archive, string filename)
    {
        logger.LogTrace("Trying to open archive, {ArchiveName}", archive);
        var zip = ZipFile.OpenRead(archive);
        {
            logger.LogDebug("Archive {ArchiveName} opened", archive);

            var entry = zip.GetEntry(filename);
            if (entry != null)
                return entry.Open();
            
            logger.LogWarning("File {FileName} in archive {ArchiveName} not found.", filename, archive);
            throw new KeyNotFoundException("File not found.");

        }
    }
}