using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Concurrent;

namespace RobotService;

public static class CleaningServiceHelper
{
    const byte Dummy = byte.MinValue;

    public static int GetUniqueVisitedPlaces(CleaningPath path)
    {
        var visited = new ConcurrentDictionary<Coordinate, byte>();
        visited.TryAdd(path.Start, Dummy);

        var x = path.Start.X;
        var y = path.Start.Y;

        Parallel.ForEach(path.Commands, (command) =>
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
                visited.TryAdd(new Coordinate { X = x, Y = y }, Dummy);
            }
        });
        return visited.Count;
    }
}