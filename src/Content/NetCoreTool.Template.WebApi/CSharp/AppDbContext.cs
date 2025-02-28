#if (HasAnyEfCoreConnector)
using Microsoft.EntityFrameworkCore;

namespace Company.WebApplication.CS;

public sealed class AppDbContext : DbContext
{
    public DbSet<ExampleEntity> Entities => Set<ExampleEntity>();

    public AppDbContext(DbContextOptions options)
        : base(options)
    {
    }
}

public sealed class ExampleEntity
{
    public long Id { get; set; }
    public string? Data { get; set; }
}
#endif
