using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.DataAccess.EF;

namespace Modules.Main;

public class TplMainContext : DbContext, IDbContext
{
    public TplMainContext(DbContextOptions<TplMainContext> options) : base(options)
    {
    }

    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        await Database.MigrateAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO Add modules configurations
    }
}