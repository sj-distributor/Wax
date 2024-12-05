namespace Wax.Core.Extensions;

public static class ModelExtensions
{
    public static PaginatedResponse<T> ToPaginatedResponse<T>(this IEnumerable<T> list, int totalCount,
        IPaginatedRequest request)
    {
        return new PaginatedResponse<T>(list, totalCount, request.PageIndex, request.PageSize);
    }

    public static PaginatedResponse<T> ToPaginatedResponse<T>(this IPaginatedList<T> list)
    {
        return new PaginatedResponse<T>(list);
    }
}