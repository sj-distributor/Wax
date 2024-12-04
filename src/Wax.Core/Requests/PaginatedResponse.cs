namespace Wax.Core.Requests;

public class PaginatedResponse<T> : IResponse
{
    public PaginatedResponse(IPaginatedList<T> paginatedList)
    {
        Items = paginatedList;
        TotalCount = paginatedList.TotalCount;
        PageIndex = paginatedList.PageIndex;
        PageSize = paginatedList.PageSize;
    }

    public PaginatedResponse(IEnumerable<T> items, int totalCount, int pageIndex, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageIndex = pageIndex;
        PageSize = pageSize;
    }

    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public IEnumerable<T> Items { get; }
}