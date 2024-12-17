namespace Shared.RequestFeature;

public abstract class RequestParamters
{
    private const int _maxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    public string? SearchTerm { get; set; }

    public string? OrderBy { get; set; }

    public string? Fields { get; set; }

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > _maxPageSize ? _maxPageSize : value;
    }
}