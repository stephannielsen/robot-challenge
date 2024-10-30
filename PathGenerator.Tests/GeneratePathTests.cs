using RobotService;

namespace PathGenerator.Tests
{
    public class GeneratePathTests
    {
        [Fact]
        public void GeneratePath_ValidPath_ReturnsPath()
        {
            var commandCount = 100;

            CleaningPathSample path = PathGeneratorHelper.GeneratePath( commandCount);

            Assert.True(path.Start.X >= -100000 && path.Start.X <= 100000);
            Assert.True(path.Start.Y >= -100000 && path.Start.Y <= 100000);
            Assert.True(path.Commands.Length <= commandCount);
            Assert.True(path.UniquePlaces > 0);
        }

        [Fact]
        public void GeneratePath_PathStaysWithinBounds_ReturnsPath()
        {
            CleaningPathSample path = PathGeneratorHelper.GeneratePath(100);

            int lastX = path.Start.X;
            int lastY = path.Start.Y;
            foreach (var command in path.Commands)
            {
                int newX = lastX;
                int newY = lastY;
                switch (command.Direction)
                {
                    case Direction.North:
                        newY += command.Steps;
                        break;
                    case Direction.East:
                        newX += command.Steps;
                        break;
                    case Direction.South:
                        newY -= command.Steps;
                        break;
                    case Direction.West:
                        newX -= command.Steps;
                        break;
                }
                Assert.True(newX >= -100000 && newX <= 100000);
                Assert.True(newY >= -100000 && newY <= 100000);
                lastX = newX;
                lastY = newY;
            }
        }

        [Fact]
        public void GeneratePath_UniquePlacesAreCorrect_ReturnsPath()
        {
            CleaningPathSample path = PathGeneratorHelper.GeneratePath(100);

            HashSet<Coordinate> visitedPoints = [path.Start];
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
                for (int j = 0; j < command.Steps; j++)
                {
                    x += dx;
                    y += dy;
                    visitedPoints.Add(new Coordinate { X = x, Y = y });
                }
            }
            Assert.Equal(visitedPoints.Count, path.UniquePlaces);
        }
    }
}