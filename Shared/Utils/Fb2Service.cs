using System.Xml;
using System.Xml.Linq;
using Shared.Models.Books;

namespace Shared.Utils;

public static class Fb2Service
{
    public static async Task<bool> ReadAsync(BookDto book, Stream stream, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        var encoding = XmlHelper.DetectEncoding(memoryStream);

        using var streamReader = new StreamReader(memoryStream, encoding);
        using var reader = XmlReader.Create(streamReader, new XmlReaderSettings { Async = true });
        var doc = await XDocument.LoadAsync(reader, LoadOptions.PreserveWhitespace, cancellationToken);
        //var doc = XDocument.Load(sgmlReader);

        try
        {
            UpdateAnnotation(book, doc);
        }
        catch (Exception)
        {
            // ignored
        }

        try
        {
            UpdateCover(book, doc);
        }
        catch (Exception)
        {
            // ignored
        }

        return true;
    }
    
    private static void UpdateAnnotation(BookDto book, XDocument doc)
    {
        var annotation = doc
            .Descendants()
            .FirstOrDefault(x => x.Name.LocalName == "annotation");
        if (annotation != null)
        {
            book.Annotation = XmlHelper.GetInnerXml(annotation);
        }
    }

    private static void ExtractImage(BookDto book, XDocument doc, XElement el)
    {
        var coverId = el.Attributes()
            .First(x => x.Name.LocalName == "href")
            .Value[1..];
        var cover = doc.Descendants()
            .First(x => x.Name.LocalName == "binary"
                        && x.Attribute("id")?.Value == coverId);
        var ctype = cover.Attribute("content-type")?.Value;
        var bin = Convert.FromBase64String(cover.Value);
        book.Cover = new BookCover
        {
            Data = bin,
            ContentType = ctype
        };
    }

    private static void UpdateCover(BookDto book, XDocument doc)
    {
        var coverPage = doc
            .Descendants()
            .Where(x => x.Name.LocalName == "coverpage")
            .Descendants()
            .FirstOrDefault(x => x.Name.LocalName == "image");

        if (coverPage != null)
        {
            ExtractImage(book, doc, coverPage);
        }
        else
        {
            var firstImage = doc
                .Descendants()
                .Where(x => x.Name.LocalName == "body")
                .Descendants()
                .FirstOrDefault(x => x.Name.LocalName == "image");
            if (firstImage != null)
            {
                ExtractImage(book, doc, firstImage);
            }
        }
    }

    public static async Task<string> GetBody(BookDto book, Stream stream, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        var encoding = XmlHelper.DetectEncoding(memoryStream);

        using var streamReader = new StreamReader(memoryStream, encoding);
        using var reader = XmlReader.Create(streamReader, new XmlReaderSettings { Async = true });
        var doc = await XDocument.LoadAsync(reader, LoadOptions.PreserveWhitespace, cancellationToken);

        var body = doc
            .Descendants()
            .FirstOrDefault(x => x.Name.LocalName == "body");

        return body is null ? string.Empty : XmlHelper.GetInnerXml(body);
    }
}