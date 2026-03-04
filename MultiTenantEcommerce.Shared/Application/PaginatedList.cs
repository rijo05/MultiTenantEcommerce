namespace MultiTenantEcommerce.Shared.Application;
public class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public long TotalCount { get; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;

    public PaginatedList(IReadOnlyCollection<T> items, long totalCount, int page, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }
}
