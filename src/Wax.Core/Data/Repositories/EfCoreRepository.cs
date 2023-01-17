using Wax.Core.Domain;
using Wax.Core.Exceptions;

namespace Wax.Core.Data.Repositories;

public class EfCoreRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly ApplicationDbContext _dbContext;

    public EfCoreRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default)
        where TKey : notnull
    {
        var entity = await _dbContext.Set<TEntity>()
            .FindAsync(new object?[] { id }, cancellationToken).ConfigureAwait(false);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity), id);
        }

        return entity;
    }

    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> InsertRangeAsync(IEnumerable<TEntity> entity,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>().AddRangeAsync(entity, cancellationToken).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return entity;
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);

        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public IQueryable<TEntity> Query => _dbContext.Set<TEntity>();
}