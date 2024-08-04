using Microsoft.AspNetCore.Components;
using Shared.Dictionaries;

namespace Common.ShaderComponents;

public partial class TableHeader : ComponentBase
{
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Width { get; set; }
    [Parameter] public required EventCallback OnClickEvent { get; set; }
    [Parameter] public string? IconName { get; set; }
    [Parameter] public SortDirection? SortDirection { get; set; }
    private string _width = string.Empty;
    private string? _sortable;
    
    protected override void OnParametersSet()
    {
        _width = string.IsNullOrEmpty(Width) ? string.Empty : $"width: {Width};";
        _sortable = SortDirection is not null ? "sortable" : string.Empty;
    }

    private Task ClickAsync()
    {
        return OnClickEvent.InvokeAsync();
    }
}