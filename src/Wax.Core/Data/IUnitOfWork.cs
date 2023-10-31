namespace Wax.Core.Data;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}

public static class UnitOfWorkExtensions
{
    public static async Task WithUnitOfWork(this IUnitOfWork uow, Func<Task> func)
    {
        await func();
        await uow.CommitAsync();
    }
}