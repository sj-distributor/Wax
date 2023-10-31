using System.Collections;

namespace Wax.Messages;

public interface IPaginatedList<out T> : IReadOnlyList<T>
{
    int PageIndex { get; }
    int TotalPages { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
}

public sealed class PaginatedList<T> : IPaginatedList<T>
{
    private readonly IReadOnlyList<T> _subset;
    public int PageIndex { get; }
    public int TotalPages { get; }

    public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        _subset = items as IReadOnlyList<T> ?? items.ToArray();
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public IEnumerator<T> GetEnumerator() => _subset.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _subset.GetEnumerator();

    public int Count => _subset.Count;

    public T this[int index] => _subset[index];

    public static PaginatedList<T> Empty()
    {
        return new PaginatedList<T>(Enumerable.Empty<T>(), 0, 0, 0);
    }
}