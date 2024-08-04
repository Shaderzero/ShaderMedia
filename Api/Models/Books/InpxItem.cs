namespace Api.Models.Books;

public class InpxItem
{
    public string? Author { get; set; }
    public string? Genre { get; set; }
    public string? Title { get; set; }
    public string? Series { get; set; }
    public string? SeriesNo { get; set; }
    public string? File { get; set; }
    public string? Size { get; set; }
    public string? LibId { get; set; }
    public string? Del { get; set; }
    public string? Ext { get; set; }
    public string? Date { get; set; }
    public string? Language { get; set; }
    public string? LibRate { get; set; }
    public string? Keyword { get; set; }

    public override string ToString()
    {
        return $"authors={Author}, genres={Genre}, title={Title}, series={Series}, serN={SeriesNo}," +
               $"file={File}, size={Size}, libId={LibId}, del={Del}, ext={Ext}, date={Date}, lang={Language}," +
               $"libRate={LibRate}, keywords={Keyword}";
    }
}