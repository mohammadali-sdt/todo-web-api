
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace TodoAPI.ContextFactory;

public class RepositoryContextFactory: IDesignTimeDbContextFactory<RepositoryContext>
{
        public RepositoryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<RepositoryContext>()
            .UseSqlite(configuration.GetConnectionString("DefaultConnection"), 
            b => b.MigrationsAssembly("TodoAPI")
            );

        return new RepositoryContext(builder.Options);
        
    }
}
