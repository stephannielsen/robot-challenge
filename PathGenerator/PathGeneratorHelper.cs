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

            (int newX, int newY) = direction switch
            { 
                Direction.North => (x, y + steps),
                Direction.East => (x + steps, y),
                Direction.South => (x, y - steps),
                Direction.West => (x - steps, y),
                _ => (x, y)
            };

            // Check if new point is in bounds of grid, otherwise skip this command and go to next one, we don't care
            if (newX < -100000 || newX > 100000 || newY < -100000 || newY > 100000)
            {
                continue;
            }
            
            // keep track of visited points for final unique places
            (int dx, int dy) = direction switch
            {
                Direction.North => (0, 1),
                Direction.East => (1, 0),
                Direction.South => (0, -1),
                Direction.West => (-1, 0),
                _ => (0, 0),
            };
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
