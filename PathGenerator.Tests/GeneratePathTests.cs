using RobotService;

namespace PathGenerator.Tests
{
    public class GeneratePathTests
    {
        [Theory]
        [InlineData(100)]
        [InlineData(10)]
        [InlineData(1)]
        [InlineData(0)]
        public void GeneratePath_ValidPath_ReturnsPath(int commandCount)
        {
            CleaningPathSample path = PathGeneratorHelper.GeneratePath(commandCount);

            Assert.True(path.Start.X >= -100000 && path.Start.X <= 100000);
            Assert.True(path.Start.Y >= -100000 && path.Start.Y <= 100000);
            Assert.True(path.Commands.Length <= commandCount);
            Assert.True(path.UniquePlaces > 0);
        }

        [Fact]
        public void GeneratePath_PathStaysWithinBounds()
        {
            CleaningPathSample path = PathGeneratorHelper.GeneratePath(100);

            int lastX = path.Start.X;
            int lastY = path.Start.Y;
            foreach (var command in path.Commands)
            {
                (int newX, int newY) = command.Direction switch
                {
                    Direction.North => (lastX, lastY + command.Steps),
                    Direction.East => (lastX + command.Steps, lastY),
                    Direction.South => (lastX, lastY - command.Steps),
                    Direction.West => (lastX - command.Steps, lastY),
                    _ => (lastX, lastY)
                };

                Assert.True(newX >= -100000 && newX <= 100000);
                Assert.True(newY >= -100000 && newY <= 100000);
                lastX = newX;
                lastY = newY;
            }
        }

        [Fact]
        public void GeneratePath_UniquePlacesAreCorrect()
        {
            CleaningPathSample path = PathGeneratorHelper.GeneratePath(100);

            HashSet<Coordinate> visitedPoints = [path.Start];
            int x = path.Start.X;
            int y = path.Start.Y;
            foreach (var command in path.Commands)
            {
                (int dx, int dy) = command.Direction switch
                {
                    Direction.North => (0, 1),
                    Direction.East => (1, 0),
                    Direction.South => (0, -1),
                    Direction.West => (-1, 0),
                    _ => (0, 0),
                };
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