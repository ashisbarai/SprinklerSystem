using SprinklerSystem.Core.Models;

namespace SprinklerSystem.Core.Abstractions
{
    public interface ISprinklerLayoutService
    {
        SprinklerLayoutResult GetPlacements(Point p1, Point p2, Point p3, Point p4, List<WaterPipe> waterPipes, double margin, double spacing);
    }
}
