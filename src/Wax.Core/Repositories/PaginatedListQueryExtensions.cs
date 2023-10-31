using Microsoft.EntityFrameworkCore;
using Wax.Messages;

namespace Wax.Core.Repositories;

public static class PaginatedListQueryExtensions
{
    public static async Task<IPaginatedList<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);

        if (count == 0)
        {
            return PaginatedList<T>.Empty();
        }
        
        var items = await source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}