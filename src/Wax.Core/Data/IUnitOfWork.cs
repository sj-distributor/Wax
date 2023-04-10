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
        return _context.HasEntitiesChanged ? _context.SaveChangesAsync(cancellationToken) : Task.CompletedTask;
    }
}