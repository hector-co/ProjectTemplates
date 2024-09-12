using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.DataAccess.EF;

namespace TplMain.Main;

public class TplMainContext(DbContextOptions<TplMainContext> options) : DbContext(options), IDbContext
{
    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        await Database.MigrateAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO Add modules configurations
    }
}