namespace Wax.Core.Extensions;

public static class QueryableExtensions
{
    public static async Task<IPaginatedList<TEntity>> ToPaginatedListAsync<TEntity>(this IQueryable<TEntity> source,
        int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);

        if (count == 0) return new PaginatedList<TEntity>([], pageIndex, pageSize);

        var items = await source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<TEntity>(items, pageIndex, pageSize, count);
    }
}