using Microsoft.EntityFrameworkCore;

namespace TplNamespace.Infrastructure
{
    public static class DependenciesConfigurator
    {
        public const string DbSchema = "tplDbSchema";

        public static void ApplyTplModuleContextConfigurations(this ModelBuilder modelBuilder, string dbSchema = DbSchema)
        {
            // TODO Add module configurations
        }
    }
}
