using PathGenerator;
using System.Text.Json;
using System.Text.Json.Serialization;


int numPaths = 10;
if (args.Length > 0)
{
    numPaths = int.Parse(args[0]);
}



string pathsDir = Path.Combine(Environment.CurrentDirectory, "paths");
if (!Directory.Exists(pathsDir))
{
    Directory.CreateDirectory(pathsDir);
}
Console.WriteLine($"Generating {numPaths} sample paths to: {pathsDir}.");

JsonSerializerOptions options = new()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
options.WriteIndented = true;

Random random = new();

for (int i = 0; i < numPaths; i++)
{
    CleaningPathSample path = PathGeneratorHelper.GeneratePath(random.Next(1, 10001));

    string json = JsonSerializer.Serialize(path, options);
    string file = Path.Combine(pathsDir, $"path_{i}.json");
    File.WriteAllText(file, json);
    Console.WriteLine($"Path written to {file}");
}