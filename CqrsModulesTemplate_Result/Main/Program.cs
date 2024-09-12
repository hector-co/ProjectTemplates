using Carter;
using MicroElements.Swashbuckle.NodaTime;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using QueryX;
using Serilog;
using Shared.Application.Behaviors;
using Shared.Infrastructure.DataAccess.EF;
using Shared.WebApi.ExceptionHandling;
using System.Text.Json;
using System.Text.Json.Serialization;
using TplMain.Main;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

static void ConfigureJsonSerializerOptions(JsonSerializerOptions serializerOptions)
{
    serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    serializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    serializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    serializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    serializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
}

builder.Services.Configure<JsonOptions>(options =>
{
    ConfigureJsonSerializerOptions(options.SerializerOptions);
});

builder.Host.UseSerilog((ctx, cfg) =>
{
    cfg.ReadFrom.Configuration(ctx.Configuration);
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    //cfg.AddOpenBehavior(typeof(RequestSessionInfoBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggerPipelineBehavior<,>));
});

builder.Services.AddCarter();

builder.Services.AddQueryX();

builder.Services.AddDbContext<IDbContext, TplMainContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("TplMain"), o => o.UseNodaTime()));

builder.Services.AddHostedService<InitDb>();

// TODO Register modules

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var jsonSerializerOptions = new JsonSerializerOptions();
    ConfigureJsonSerializerOptions(jsonSerializerOptions);
    c.ConfigureForNodaTimeWithSystemTextJson(jsonSerializerOptions);
});

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlerMiddleware>();

//app.UseAuthorization();

app.MapCarter();

app.Run();

public partial class Program { }
