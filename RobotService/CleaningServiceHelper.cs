using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Concurrent;
using System.IO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RobotService;

public static class CleaningServiceHelper
{
    public static int GetUniqueVisitedPlaces(CleaningPath path)
    {
        HashSet<Coordinate> visited = [path.Start];

        int x = path.Start.X;
        int y = path.Start.Y;

        foreach (var command in path.Commands)
        {
            (int dx, int dy) = GetDirectionChange(command.Direction);

            for (int i = 0; i < command.Steps; i++)
            {
                x += dx;
                y += dy;
                visited.Add(new Coordinate { X = x, Y = y });
            }
        }

        return visited.Count;
    }

    static (int dx, int dy) GetDirectionChange(Direction direction)
    {
        return direction switch
        {
            Direction.North => (0, 1),
            Direction.East => (1, 0),
            Direction.South => (0, -1),
            Direction.West => (-1, 0),
            _ => (0, 0),
        };
    }
}