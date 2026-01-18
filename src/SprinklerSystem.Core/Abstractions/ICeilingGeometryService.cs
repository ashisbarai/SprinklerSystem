using SprinklerSystem.Core.Models;

namespace SprinklerSystem.Core.Abstractions
{
    public interface ICeilingGeometryService
    {
        GridResult GenerateGrid(Point p1, Point p2, Point p3, Point p4, double margin, double spacing);
        Point GetConnectionPoint(Point sprinkler, WaterPipe pipe);
        (Point connectionPoint, string pipeID) GetNearestPipe(Point sprinkler, List<WaterPipe> pipes);
    }
}
