using Wax.Core.Domain;
using Wax.Core.Exceptions;

namespace Wax.Core.Data.Repositories;

public class EfCoreRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    private readonly ApplicationDbContext _dbContext;

    public EfCoreRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TEntity> GetByIdAsync(TKey id)
    {
        var entity = await _dbContext.Set<TEntity>().FindAsync(id).ConfigureAwait(false);

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity), id);
        }

        return entity;
    }

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        return entity;
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}