using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ICleaningService, CleaningService>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

app.MapGet("/", () => "Hello, please use the API :)");

app.MapPost("/tibber-developer-test/enter-path", async (CleaningPath path, ICleaningService service) => await service.Calculate(path));

app.Run();
