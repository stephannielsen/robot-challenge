using System.Collections;

namespace RobotService;

public static class CleaningServiceHelper
{
    const int GRID_MAX = 200000;
    const int START_OFFSET = GRID_MAX / 2;

    public static int GetUniqueVisitedPlaces(CleaningPath path)
    {
        var visited = new Dictionary<int, BitArray>();

        int x = path.Start.X;
        int y = path.Start.Y;

        int uniquePlaces = 0;

        AddVisited(x, y, ref visited, ref uniquePlaces);

        foreach (var command in path.Commands)
        {
            (int dx, int dy) = GetDirectionChange(command.Direction);

            for (int i = 0; i < command.Steps; i++)
            {
                x += dx;
                y += dy;
                AddVisited(x, y, ref visited, ref uniquePlaces);
            }
        }

        return uniquePlaces;
    }

    static void AddVisited(int x, int y, ref Dictionary<int, BitArray> visited, ref int uniquePlaces)
    {
        if (!visited.TryGetValue(x, out var visitedYCoordinates))
        {
            visitedYCoordinates = new BitArray(GRID_MAX);
            visited[x] = visitedYCoordinates;
        }
        if (!visitedYCoordinates[y + START_OFFSET])
        {
            visitedYCoordinates[y + START_OFFSET] = true;
            uniquePlaces++;
        }
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