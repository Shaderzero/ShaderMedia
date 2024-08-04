namespace Common.Models;

public class DownloadedFile
{
    public string Filename { get; set; }
    public string MeidaType { get; set; }
    public Stream Data { get; set; }
}