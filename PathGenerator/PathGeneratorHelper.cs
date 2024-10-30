using RobotService;

namespace PathGenerator;

public record CleaningPathSample : CleaningPath
{
    public int UniquePlaces { get; init; }
}

public class PathGeneratorHelper
{
    public static CleaningPathSample GeneratePath(int commandCount)
    {
        Random random = new();
        var start = new Coordinate { X = random.Next(-100000, 100001), Y = random.Next(-100000, 100001) };
        HashSet<Coordinate> visitedPoints = [start];
        List<Command> commands = [];

        int x = start.X;
        int y = start.Y;

        for (int i = 0; i < commandCount; i++)
        {
            Direction direction = (Direction)random.Next(4);
            int steps = random.Next(1, 100000);

            int newX = x;
            int newY = y;
            switch (direction)
            {
                case Direction.North:
                    newY += steps;
                    break;
                case Direction.East:
                    newX += steps;
                    break;
                case Direction.South:
                    newY -= steps;
                    break;
                case Direction.West:
                    newX -= steps;
                    break;
            }

            if (newX < -100000 || newX > 100000 || newY < -100000 || newY > 100000)
            {
                continue;
            }

            int dx = 0, dy = 0;
            switch (direction)
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
            for (int j = 0; j < steps; j++)
            {
                x += dx;
                y += dy;
                visitedPoints.Add(new Coordinate { X = x, Y = y });
            }
            commands.Add(new Command { Direction = direction, Steps = steps });
        }

        return new CleaningPathSample { Start = visitedPoints.First(), Commands = [.. commands], UniquePlaces = visitedPoints.Count };
    }

}
