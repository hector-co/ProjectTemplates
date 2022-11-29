using Npgsql;
using Respawn;
using Respawn.Graph;

namespace TplNamespace.WebApi.Tests.Common;

public class DatabaseManager
{
    public DatabaseManager(string connectionString)
    {
        using var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        var respawner = Respawner.CreateAsync(conn, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            TablesToIgnore = new Table[]
            {
                        "__EFMigrationsHistory"
            },
            SchemasToInclude = new[] { "tplModule" },
        }).Result;

        respawner.ResetAsync(conn).Wait();
    }
}
