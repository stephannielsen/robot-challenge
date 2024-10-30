using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Concurrent;

namespace RobotService;

public static class CleaningServiceHelper
{
    public static int GetUniqueVisitedPlaces(CleaningPath path)
    {
        var visited = new HashSet<Coordinate> { path.Start };

        int x = path.Start.X;
        int y = path.Start.Y;

        foreach (var command in path.Commands)
        {
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