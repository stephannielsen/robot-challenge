using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using RobotService;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContextPool<RobotDb>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("RobotDb"));
    opt.UseSnakeCaseNamingConvention();
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddTransient<ICleaningService, CleaningService>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "CleaningRobotAPI";
    config.Title = "Cleaning Robot API v1";
    config.Version = "v1";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(exceptionHandler => exceptionHandler.Run(async context => await Results.Problem().ExecuteAsync(context)));
}

app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.DocumentTitle = "CleaningRobotAPI";
    config.Path = "";
    config.DocumentPath = "/swagger/{documentName}/swagger.json";
    config.DocExpansion = "list";
});

app.MapPost("/tibber-developer-test/enter-path", ApiHandler.CalculateCleaningPath).AddEndpointFilter<CleaningPathIsValidFilter>();

app.Run();
public partial class Program
{ }
