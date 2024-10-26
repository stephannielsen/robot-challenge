namespace RobotService;

public static class CleaningServiceHelper
{
    public static int GetUniqueVisitedPlaces(CleaningPath path)
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