using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TplNamespace.Infrastructure.DataAccess.EF;

public class InitData : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public InitData(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IDbContext>();
        var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

        await context.MigrateAsync(cancellationToken);

        await AddData(context, env, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static async Task AddData(IDbContext context, IHostEnvironment environment, CancellationToken cancellationToken)
    {
    }
}

