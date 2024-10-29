using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Concurrent;

namespace RobotService;

public static class CleaningServiceHelper
{
    public static int GetUniqueVisitedPlaces(CleaningPath path)
    {
        var visited = new ConcurrentHashSet<Coordinate>(path.Commands.Length);
        visited.Add(path.Start);

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
                visited.Add(new Coordinate { X = x, Y = y });
            }
        });
        return visited.Count;
    }
}