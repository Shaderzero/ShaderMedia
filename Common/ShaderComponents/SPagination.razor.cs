using Microsoft.AspNetCore.Components;
using Shared.Models;

namespace Common.ShaderComponents;

public partial class SPagination : ComponentBase
{
    [Parameter] public required Pagination Pagination { get; set; }
    [Parameter] public required EventCallback<Pagination> OnPageSelect { get; set; }
    private Pagination _pagination = new();
    private int _pageCount = 1;
    private int[] _pageNumbers = [];

    protected override void OnParametersSet()
    {
        _pagination = Pagination;
        CountPages();
        GeneratePages();
    }
    
    private void GoPage(int pageNumber)
    {
        if (pageNumber < 1)
            _pagination.PageNumber = 1;
        if (pageNumber > _pageCount)
            _pagination.PageNumber = _pageCount;
        _pagination.PageNumber = pageNumber;
        
        OnPageSelect.InvokeAsync(_pagination);
    }

    private string GetActive(int num) => num == _pagination.PageNumber ? "active" : string.Empty;

    private void GeneratePages()
    {
        var currentPage = _pagination.PageNumber;
        var set = new HashSet<int>();

        for (var i = 0; i < 5; i++)
        {
            var x = 0;
            if (currentPage < 3)
                x = 1 + i;
            else if (currentPage > _pageCount - 3)
                x = _pageCount - i;
            else 
                x = currentPage - 2 + i;
            if (x > 0 && x <= _pageCount)
                set.Add(x);
        }


        set.Add(1);
        set.Add(_pageCount);

        _pageNumbers = set.Order().ToArray();
    }
    
    private void CountPages()
    {
        var total = Pagination.Count / Pagination.PageSize;
        if (Pagination.Count % Pagination.PageSize == 0)
        {
            _pageCount = total;
        }

        _pageCount = total + 1;
    }
}