using Microsoft.EntityFrameworkCore;

namespace TplNamespace.Infrastructure.DataAccess.EF;

public class TplModuleContext : DbContext
{
    public const string DbSchema = "tplModule";

    public TplModuleContext(DbContextOptions<TplModuleContext> options) : base(options)
    {
    }
}

