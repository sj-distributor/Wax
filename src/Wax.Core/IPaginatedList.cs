namespace Wax.Core;

public interface IPaginatedList<T> : IList<T>
{
    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
}

public sealed class PaginatedList<T> : List<T>, IPaginatedList<T>
{
    public PaginatedList(IList<T> source, int pageIndex, int pageSize, int? totalCount = null)
    {
        //min allowed page size is 1
        pageSize = Math.Max(pageSize, 1);

        TotalCount = totalCount ?? source.Count;

        PageSize = pageSize;
        PageIndex = pageIndex;
        AddRange(totalCount != null ? source : source.Skip(pageIndex * pageSize).Take(pageSize));
    }


    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
}