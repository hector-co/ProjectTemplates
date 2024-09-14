using Microsoft.OpenApi.Models;
using Shared.Application;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json.Serialization;

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

public class SwaggerIgnoreFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext schemaFilterContext)
    {
        if (schema.Properties.Count == 0)
        {
            return;
        }
        var properties = schemaFilterContext.Type.GetProperties();

        var excludedList = properties
            .Where(m => m.GetCustomAttribute<SwaggerIgnoreAttribute>() is not null)
            .Select(m => m.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? ToCamelCase(m.Name));

        foreach (var excludedName in excludedList)
        {
            schema.Properties.Remove(excludedName);
        }
    }

    private static string ToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || char.IsLower(input[0]))
        {
            return input;
        }
        var chars = input.ToCharArray();
        chars[0] = char.ToLowerInvariant(input[0]);

        return new(chars);
    }
}
