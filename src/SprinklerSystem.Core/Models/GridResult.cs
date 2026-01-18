namespace SprinklerSystem.Core.Models
{
    public class GridResult(int numberOfSprinklers, List<Point> points)
    {
        public int NumberOfSprinklers { get; init; } = numberOfSprinklers;
        public List<Point> Points { get; init; } = points;
    }
}
