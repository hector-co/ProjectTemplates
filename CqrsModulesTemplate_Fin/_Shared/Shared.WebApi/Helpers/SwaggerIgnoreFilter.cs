using Microsoft.OpenApi.Models;
using Shared.Application;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Shared.WebApi.Helpers;

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
