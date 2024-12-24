using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;

namespace Repository
{
    public class RepositoryContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {

        public RepositoryContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // modelBuilder.ApplyConfiguration(new RoleConfiguration());
            // modelBuilder.ApplyConfiguration(new UserConfiguration());
            // modelBuilder.ApplyConfiguration(new TodoConfiguration());
        }

        //
        // public DbSet<User>? Users { get; set; }
        public DbSet<Todo>? Todos { get; set; }
    }
}
