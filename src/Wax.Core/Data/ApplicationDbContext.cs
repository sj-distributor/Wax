using Microsoft.EntityFrameworkCore;
using Wax.Core.Data.Configurations;

namespace Wax.Core.Data;

public class ApplicationDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerEntityTypeConfiguration).Assembly);
    }
}