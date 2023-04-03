using TplNamespace.Infrastructure.DataAccess.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TplNamespace.WebApi.Tests.Common;

public class TestServerFixture : WebApplicationFactory<Program>, IDisposable
{
    public string ConnectionString { get; private set; } = string.Empty;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            ConnectionString = services.BuildServiceProvider().GetRequiredService<IConfiguration>().GetConnectionString("TplModule");

            var context = services.BuildServiceProvider().GetRequiredService<TplModuleContext>();
            context.Database.EnsureDeleted();
        });
    }
}