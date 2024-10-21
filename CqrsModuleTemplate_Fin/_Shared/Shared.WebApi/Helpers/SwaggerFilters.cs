using Microsoft.OpenApi.Models;
using Shared.Application;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Shared.WebApi.Helpers;

public class SwaggerParameterIgnoreFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var properties = context.MethodInfo.GetParameters()
            .SelectMany(p => p.ParameterType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            .Where(p => p.GetCustomAttribute<SwaggerIgnoreAttribute>() is not null);

        foreach (var property in properties)
        {
            var parameter = context.ApiDescription.ParameterDescriptions.FirstOrDefault(p => p.Name == property.Name);
            if (parameter is not null)
            {
                operation.RequestBody = null;
                context.ApiDescription.ParameterDescriptions.Remove(parameter);
            }
        }
    }
}
