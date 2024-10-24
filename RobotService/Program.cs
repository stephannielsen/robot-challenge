using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

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
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "CleaningRobotAPI";
    config.Title = "Cleaning Robot API v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "CleaningRobotAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/", () => "Hello, please use the API :)");

app.MapPost("/tibber-developer-test/enter-path", async (CleaningPath path, ICleaningService service, RobotDb db) =>
{
    var result = await service.Calculate(path, db);
    return Results.Created($"/results/{result.ID}", result);
});

app.Run();
