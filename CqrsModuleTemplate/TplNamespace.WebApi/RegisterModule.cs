using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
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
                //.AddValidatorsFromAssemblyContaining(typeof(RegisterTplModuleValidator))
                ;

        //builder.Services.AddMediatR(cfg =>
        //{
        //    cfg.RegisterServicesFromAssemblies(typeof(RegisterTplModuleHandler).Assembly);
        //});

        builder.Services.AddHostedService<InitData>();
    }
}
