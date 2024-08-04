namespace Shared;

public static class ApiPaths
{
    private const string Prefix = "/api/v1/";

    private const string Account = Prefix + "account/";
    public const string UserRegister = Account + "register";
    public const string Users = Account + "users";
    
    private const string Inpx = Prefix + "inpx/";
    public const string InpxInit = Inpx + "init";
    
    public const string Books = Prefix + "books";
    public const string BookId = "{bookId}";
    public const string BookInfo = Books + "/info/" + BookId;
    public const string BookDownload = Books + "/download/" + BookId;
}