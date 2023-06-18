using System.Text.Json.Serialization;
using System.Text.Json;
using Shared.Application.Behaviors;
using Serilog;
using QueryX;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.DataAccess.EF;
using TplMain.Main;
using Shared.WebApi.ExceptionHandling;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .AddControllersAsServices();

builder.Host.UseSerilog((ctx, cfg) =>
{
    cfg.ReadFrom.Configuration(ctx.Configuration);
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggerPipelineBehavior<,>));
});

builder.Services.AddQueryX();

builder.Services.AddCors();

builder.Services.AddDbContext<IDbContext, TplMainContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("TplMain")));

builder.Services.AddHostedService<InitDb>();

// TODO Register modules

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
