using Microsoft.EntityFrameworkCore;

namespace TplNamespace.Infrastructure.DataAccess.EF;

public class TplModuleContext : DbContext, ITplModuleContext
{
    public const string DbSchema = "tplDbSchema";

    public TplModuleContext(DbContextOptions<TplModuleContext> options) : base(options)
    {
    }

    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        await Database.MigrateAsync(cancellationToken);
    }
}

