namespace Shared.Models;

public class ApiResponse<T>
{
    public T? Value { get; set; }
    public string[] Errors { get; set; } = [];
    public string Info { get; set; } = "";
}