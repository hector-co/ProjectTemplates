using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QueryX;
using Serilog;
using TplNamespace.Application.Common.Behaviors;
using TplNamespace.Infrastructure.DataAccess.EF;

namespace TplNamespace.Infrastructure
{
    public static class DependenciesConfigurator
    {
        public static WebApplicationBuilder RegisterDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<TplModuleContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("TplModule"),
                    o => o.MigrationsHistoryTable("__EFMigrationsHistory", TplModuleContext.DbSchema)));

            builder.Services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining(typeof(ValidationBehavior<,>));

            builder.Services.AddMediatR(typeof(TplModuleContext)/*, typeof(TplModuleCommand)*/);
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            builder.Services.AddQueryX();

            builder.Logging.ClearProviders();
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            builder.Logging.AddSerilog(logger);

            builder.Services.AddHostedService<InitData>();

            return builder;
        }
    }
}
