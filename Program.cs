using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

app.MapPost("/tibber-developer-test/enter-path", (CleaningPath path) => new CleaningResult{ ID = "123", Timestamp = DateTime.Now, Commands = 2, Result = 4, Duration = 0.00123 });

app.Run();
