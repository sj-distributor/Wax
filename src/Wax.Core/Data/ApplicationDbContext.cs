using Microsoft.EntityFrameworkCore;
using Wax.Core.Data.Configurations;

namespace Wax.Core.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("__wax_database");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerEntityTypeConfiguration).Assembly);
    }
    
    public bool HasEntitiesChanged { get; private set; }
    
    public async Task ChangeEntitiesAsync(bool saveNow = false, CancellationToken cancellationToken = default)
    {
        HasEntitiesChanged = true;

        if (saveNow)
        {
            await SaveChangesAsync(cancellationToken);
        }
    }
}