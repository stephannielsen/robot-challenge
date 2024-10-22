using System.Diagnostics;

public class CleaningService : ICleaningService
{
    private readonly TimeProvider _timeProvider;

    public CleaningService(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public async Task<CleaningResult> Calculate(CleaningPath path)
    {
        var stopwatch = Stopwatch.StartNew();
        var visitedPlaces = VisitedPlaces(path);
        stopwatch.Stop();
        return await Task.FromResult(new CleaningResult { ID = "1243", Timestamp = _timeProvider.GetUtcNow().DateTime, Commands = path.Commands.Length, Result = visitedPlaces, Duration = stopwatch.Elapsed.TotalSeconds });
    }

    private int VisitedPlaces(CleaningPath path)
    {
        var visited = new HashSet<Coordinate>
        {
            path.Start
        };

        var x = path.Start.X;
        var y = path.Start.Y;

        foreach (var command in path.Commands)
        {
            // determine direction once for all steps
            int dx = 0, dy = 0;
            switch (command.Direction)
            {
                case Direction.North:
                    dy = 1;
                    break;
                case Direction.East:
                    dx = 1;
                    break;
                case Direction.South:
                    dy = -1;
                    break;
                case Direction.West:
                    dx = -1;
                    break;
            }

            for (var i = 0; i < command.Steps; i++)
            {
                x += dx;
                y += dy;
                visited.Add(new Coordinate { X = x, Y = y });
            }
        }
        return visited.Count;
    }
}