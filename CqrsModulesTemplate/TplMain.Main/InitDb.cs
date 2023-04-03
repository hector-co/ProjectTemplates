using Microsoft.EntityFrameworkCore;
using Modules.Main;

namespace TplMain.Main;

public class InitDb : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public InitDb(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TplMainContext>();
        var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

        await context.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
