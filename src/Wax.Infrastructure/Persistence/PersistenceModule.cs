namespace Wax.Infrastructure.Persistence;

public class PersistenceModule: Module
{
    private readonly string _connectionString;

    public PersistenceModule(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(c =>
            {
                if (!string.IsNullOrEmpty(_connectionString))
                {
                    //Select your database provider
                }

                var optionBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                optionBuilder.UseInMemoryDatabase("__wax_database");

                return new ApplicationDbContext(optionBuilder.Options);
            }).AsSelf().As<IApplicationDbContext>()
            .InstancePerLifetimeScope();
    }
}