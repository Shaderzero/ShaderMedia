using System.IO.Compression;
using System.Text;
using Api.Data.Books;
using Api.Models.Books;
using Api.Repositories.Interfaces.Books;
using Api.Services.Interfaces;

namespace Api.Services;

public class ZipService(IAuthorRepository authorRepository, 
    IGenreRepository genreRepository,
    ILanguageRepository languageRepository,
    ISerieRepository serieRepository, IKeywordRepository keywordRepository,
    IBookRepository bookRepository, ILogger<ZipService> logger) : IZipService
{
    private const string FilePath = "./LibraryDir/flibusta_fb2_local.inpx";
    private const string CollectionInfo = "collection.info";
    private const string VersionInfo = "version.info";
    private const string DefaultStructure = "AUTHOR;GENRE;TITLE;SERIES;SERNO;FILE;SIZE;LIBID;DEL;EXT;DATE;LANG;KEYWORDS";

    private readonly Dictionary<int, Author> _authors = [];
    private readonly Dictionary<int, Genre> _genres = [];
    private readonly Dictionary<int, Series> _series = [];
    private readonly Dictionary<int, Language> _languages = [];
    private readonly Dictionary<int, Keyword> _keywords = [];
    private readonly Dictionary<int, Book> _books = [];
    private readonly Dictionary<string, BookAuthor> _bookAuthors = [];
    private readonly Dictionary<string, BookGenre> _bookGenres = [];
    private readonly Dictionary<string, BookKeyword> _bookKeywords = [];

    public async Task ParseZipAsync()
    {
        var startTime = DateTime.Now;
        using var zips = ZipFile.OpenRead(FilePath);
        foreach (var zip in zips.Entries)
        {
            if (zip.Name is not (CollectionInfo or VersionInfo))
                await ParseInpxAsync(zip);
        }

        var endTime = DateTime.Now;
        var timeProcess = endTime - startTime;
        logger.LogWarning("Time of processing={time}", timeProcess.ToString("g"));
        logger.LogWarning("Books count={count}, author={authors}, genres={genres}, " +
                          "series={series}, lanuages={lanuages}", 
            _books.Count, _authors.Count, _genres.Count, 
            _series.Count, _languages.Count);

        startTime = DateTime.Now;
        await SaveToDb();
        endTime = DateTime.Now;
        timeProcess = endTime - startTime;
        logger.LogWarning("Time of processing={time}", timeProcess.ToString("g"));
        _authors.Clear();
        _genres.Clear();
        _series.Clear();
        _books.Clear();
        _bookAuthors.Clear();
        _bookGenres.Clear();
        _bookKeywords.Clear();
        GetFileContent(VersionInfo);
        GetFileContent(CollectionInfo);
    }

    private async Task SaveToDb()
    {
        try
        {
            await SaveToTable(authorRepository.BulkInsertAsync(_authors.Values), "Authors");
            await SaveToTable(genreRepository.BulkInsertAsync(_genres.Values), "Genres");
            await SaveToTable(languageRepository.BulkInsertAsync(_languages.Values), "Languages");
            await SaveToTable(serieRepository.BulkInsertAsync(_series.Values), "Series");
            await SaveToTable(keywordRepository.BulkInsertAsync(_keywords.Values), "Keywords");
            await SaveToTable(bookRepository.BulkInsertAsync(_books.Values), "Books");
            await SaveToTable(authorRepository.BulkInsertAsync(_bookAuthors.Values), "Book Authors");
            await SaveToTable(genreRepository.BulkInsertAsync(_bookGenres.Values), "Book Genres");
            await SaveToTable(keywordRepository.BulkInsertAsync(_bookKeywords.Values), "Book Keywords");
        }
        catch (Exception ex)
        {
            
        }
    }

    private async Task SaveToTable(Task task, string name)
    {
        var startTime = DateTime.Now;
        await task;
        var timeNow = DateTime.Now;
        var timeProcess = timeNow - startTime;
        logger.LogWarning("{name} inserted with time={time}", name, timeProcess.ToString("g"));
    }

    public string? GetFileContent(string fileName)
    {
        using var zips = ZipFile.OpenRead(FilePath);
        var infoZip = zips.Entries.FirstOrDefault(x => x.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase));
        if (infoZip is null)
            return string.Empty;

        using var stream = infoZip.Open();
        var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        return content;
    }

    public async Task ParseInpxAsync(ZipArchiveEntry zip)
    {
        await using var stream = zip.Open();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        // var lineCount = CountLinesInFile(reader);
        var zipName = zip.Name.Replace(".inp", ".zip");
        while (await reader.ReadLineAsync() is { } line)
        {
            var parts = line.Split('\x04');
            SaveToMemory(CreateInpxItem(parts), zipName);
        }
    }

    private static InpxItem CreateInpxItem(string[] parts)
    {
        var item = new InpxItem
        {
            Author = parts.ElementAtOrDefault(0),
            Genre = parts.ElementAtOrDefault(1),
            Title = parts.ElementAtOrDefault(2),
            Series = parts.ElementAtOrDefault(3),
            SeriesNo = parts.ElementAtOrDefault(4),
            File = parts.ElementAtOrDefault(5),
            Size = parts.ElementAtOrDefault(6),
            LibId = parts.ElementAtOrDefault(7),
            Del = parts.ElementAtOrDefault(8),
            Ext = parts.ElementAtOrDefault(9),
            Date = parts.ElementAtOrDefault(10),
            Language = parts.ElementAtOrDefault(11),
            LibRate = parts.ElementAtOrDefault(12),
            Keyword = parts.ElementAtOrDefault(13)
        };
        return item;
    }

    private void SaveToMemory(InpxItem item, string zipName)
    {
        var libId = ParseStringToInt(item.LibId);
        if (libId is null || _books.TryGetValue(libId.Value, out _))
            return;
        
        var serie = CreateSerie(item.Series);
        var language = CreateLanguage(item.Language);
        var book = new Book
        {
            Id = libId.Value,
            ZipName = zipName,
            // Authors = CreateBookAuthors(id.Value, item.Authors),
            // Genres = CreateBookGenres(id.Value, item.Genres),
            Title = item.Title ?? string.Empty,
            SeriesId = serie?.Id,
            SeriesNo = ParseStringToInt(item.SeriesNo),
            File = ParseStringToInt(item.File) ?? -1,
            Size = ParseStringToInt(item.Size) ?? -1,
            Del = ParseStringToInt(item.Del) == 1,
            Ext = item.Ext,
            Date = ParseStringToDate(item.Date),
            LanguageId = language?.Id
        };
        _books[libId.Value] =  book;
        CreateBookAuthors(libId.Value, item.Author);
        CreateBookGenres(libId.Value, item.Genre);
        CreateBookKeywords(libId.Value, item.Keyword);
    }

    private static int? ParseStringToInt(string? str)
    {
        if (int.TryParse(str, out var num))
            return num;
        return null;
    }
    
    private static DateOnly? ParseStringToDate(string? str)
    {
        if (DateTime.TryParse(str, out var date))
        {
            return DateOnly.FromDateTime(date);
        }
        return null;
    }

    private void CreateBookAuthors(int bookId, string? authors)
    {
        if (string.IsNullOrEmpty(authors))
            return;
        var array = GetArrayFromDelimitedText(':' ,authors);
        foreach (var str in array)
        {
            if (string.IsNullOrEmpty(str))
                continue;
            
            var author = GetOrCreateAuthor(ParseStringToAuthor(str));
            var key = $"{bookId}_{author.Id}";
            _bookAuthors[key] = new BookAuthor { BookId = bookId, AuthorId = author.Id };
        }
    }

    private void CreateBookGenres(int bookId, string? genres)
    {
        if (string.IsNullOrEmpty(genres))
            return;
        var array = GetArrayFromDelimitedText(':', genres);
        foreach (var str in array)
        {
            if (string.IsNullOrEmpty(str))
                continue;
            
            var genre = GetOrCreateGenre(ParseStringToGenre(str));
            var key = $"{bookId}_{genre.Id}";
            _bookGenres[key] = new BookGenre { BookId = bookId, GenreId = genre.Id };
        }
    }
    
    private void CreateBookKeywords(int bookId, string? keywords)
    {
        if (string.IsNullOrEmpty(keywords))
            return;
        var array = GetArrayFromDelimitedText(':', keywords);
        foreach (var str in array)
        {
            if (string.IsNullOrEmpty(str))
                continue;
            var keyword = GetOrCreateKeyword(ParseStringToKeyword(str));
            var key = $"{bookId}_{keyword.Id}";
            _bookKeywords[key] = new BookKeyword { BookId = bookId, KeywordId = keyword.Id };
        }
    }

    private Series? CreateSerie(string? str)
    {
        if (string.IsNullOrEmpty(str))
            return null;

        var serie = ParseStringToSerie(str);
        serie = GetOrCreateSerie(serie);

        return serie;
    }
    
    private Language? CreateLanguage(string? str)
    {
        if (string.IsNullOrEmpty(str))
            return null;

        var language = ParseStringToLanguage(str);
        language = GetOrCreateLanguage(language);

        return language;
    }
    
    private Author GetOrCreateAuthor(Author author)
    {
        var key = author.GenerateHash();
        if (_authors.TryGetValue(key, out var existedValue))
            return existedValue;

        author.Id = key;
        _authors[key] = author;
        return author;
    }

    private Genre GetOrCreateGenre(Genre genre)
    {
        var key = genre.GenerateHash();
        if (_genres.TryGetValue(key, out var existedValue))
            return existedValue;
        
        genre.Id = key;
        _genres[key] = genre;
        return genre;
    }
    
    private Keyword GetOrCreateKeyword(Keyword keyword)
    {
        var key = keyword.GenerateHash();
        if (_keywords.TryGetValue(key, out var existedValue))
            return existedValue;
        
        keyword.Id = key;
        _keywords[key] = keyword;
        return keyword;
    }

    private Series GetOrCreateSerie(Series series)
    {
        var key = series.GenerateHash();
        if (_series.TryGetValue(key, out var existedValue))
            return existedValue;
        
        series.Id = key;
        _series[key] = series;
        return series;
    }
    
    private Language GetOrCreateLanguage(Language language)
    {
        var key = language.GenerateHash();
        if (_languages.TryGetValue(key, out var existedValue))
            return existedValue;
        
        language.Id = key;
        _languages[key] = language;
        return language;
    }

    private static Author ParseStringToAuthor(string author)
    {
        var authorValues = GetArrayFromDelimitedText(',',author);

        return new Author
        {
            LastName = authorValues.ElementAtOrDefault(0)!,
            FirstName = authorValues.ElementAtOrDefault(1),
            MiddleName = authorValues.ElementAtOrDefault(2)
        };
    }

    private static Genre ParseStringToGenre(string genre)
    {
        return new Genre
        {
            Name = genre,
        };
    }
    
    private static Keyword ParseStringToKeyword(string keyword)
    {
        return new Keyword
        {
            Name = keyword,
        };
    }

    private static Series ParseStringToSerie(string serie)
    {
        return new Series
        {
            Name = serie,
        };
    }
    
    private static Language ParseStringToLanguage(string language)
    {
        language = (language.Length > 2 ? language[..2] : language).ToLowerInvariant();
        return new Language
        {
            Name = language,
        };
    }
    
    private static string[] GetArrayFromDelimitedText(char delimiter, string text, bool removeEmpty = true) =>
        text
            .Split(delimiter)
            .Where(x => !removeEmpty || (!string.IsNullOrEmpty(x) || x == ","))
            .ToArray();
}