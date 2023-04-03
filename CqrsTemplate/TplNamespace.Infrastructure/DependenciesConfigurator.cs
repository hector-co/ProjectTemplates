using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QueryX;
using Serilog;
using TplNamespace.Infrastructure.DataAccess.EF;

namespace TplNamespace.Infrastructure
{
    public static class DependenciesConfigurator
    {
        public static WebApplicationBuilder RegisterDependencies(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<IDbContext, TplModuleContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("TplModule"),
                    o => o.MigrationsHistoryTable("__EFMigrationsHistory", TplModuleContext.DbSchema)));

            builder.Services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining(typeof(ValidationPipelineBehavior<,>));

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(TplModuleContext).Assembly, typeof(ValidationPipelineBehavior<,>).Assembly);
                cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggerPipelineBehavior<,>));
            });

            builder.Services.AddQueryX();

            builder.Host.UseSerilog((ctx, cfg) =>
            {
                cfg.ReadFrom.Configuration(ctx.Configuration);
            });

            builder.Services.AddHostedService<InitData>();

            return builder;
        }
    }
}
