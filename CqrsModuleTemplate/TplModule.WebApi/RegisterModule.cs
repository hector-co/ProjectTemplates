using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TplNamespace.Application;
using TplNamespace.Infrastructure;
using TplNamespace.Infrastructure.DataAccess.EF;

namespace TplNamespace.WebApi;

public static class RegisterModule
{
    public static void RegisterTplModule(this WebApplicationBuilder builder)
    {
        var assembly = typeof(RegisterModule).Assembly;
        builder.Services.AddControllers().AddApplicationPart(assembly);

        builder.Services
                .AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining(typeof(ApplicationAssemblyReference))
                ;

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(InfrastructureAssemblyReference).Assembly);
        });

        builder.Services.AddHostedService<InitData>();
    }
}
