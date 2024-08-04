using System.Web;

namespace Common.Utils;

public static class RouteHelper
{
    public static string ReplaceRouteParameters(string route, IReadOnlyDictionary<string, string> nameReplacements)
    {
        string result = route;
        foreach ((string name, string replacement) in nameReplacements)
        {
            result = ReplaceRouteParameter(result, name, replacement);
        }
        return result;
    }

    public static string ReplaceRouteParameter(string route, string name, string replacement)
        => route.Replace($"{{{name}}}", HttpUtility.UrlEncode(replacement));

    public static string AppendQueryString(string route, Dictionary<string, string> parameters)
    {
        var pairs = parameters.Select(x => $"{HttpUtility.UrlEncode(x.Key)}={HttpUtility.UrlEncode(x.Value)}");
        var query = string.Join('&', pairs);
        var separator = route.Contains('?', StringComparison.Ordinal)
            ? string.Empty
            : "?";
        var resultUrl = route + separator + query;
        return resultUrl;
    }
}