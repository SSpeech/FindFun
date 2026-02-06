using FindFun.Server.Domain;
using Microsoft.EntityFrameworkCore;

namespace FindFun.Server.Infrastructure;

public class FindFunDbContext(DbContextOptions<FindFunDbContext> options) : DbContext(options)
{
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Street> Streets => Set<Street>();
    public DbSet<Park> Parks => Set<Park>();
    public DbSet<Municipality> Municipalities => Set<Municipality>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FindFunDbContext).Assembly);
    }
}
