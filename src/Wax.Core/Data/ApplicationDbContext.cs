﻿using Microsoft.EntityFrameworkCore;
using Wax.Core.Data.Configurations;
using Wax.Core.Repositories;

namespace Wax.Core.Data;

public class ApplicationDbContext : DbContext, IUnitOfWork
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
}