using Mediator.Net.Contracts;

namespace Wax.Messages.Requests;

public class PaginatedResponse<T> : IResponse
{
    public PaginatedResponse(IPaginatedList<T> list)
    {
        PageIndex = list.PageIndex;
        TotalPages = list.TotalPages;
        HasPreviousPage = list.HasPreviousPage;
        HasNextPage = list.HasNextPage;
        Data = list;
    }

    public int PageIndex { get; }
    public int TotalPages { get; }
    public bool HasPreviousPage { get; }
    public bool HasNextPage { get; }
    public IPaginatedList<T> Data { get; }
}