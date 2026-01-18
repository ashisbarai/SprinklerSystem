using SprinklerSystem.Core.Abstractions;
using SprinklerSystem.Core.Models;

namespace SprinklerSystem.Core.Services
{
    public class SprinklerLayoutService(ICeilingGeometryService ceilingGeometryService) : ISprinklerLayoutService
    {
        public SprinklerLayoutResult GetPlacements(Point p1, Point p2, Point p3, Point p4, List<WaterPipe> waterPipes, double margin, double spacing)
        {
            var results = new List<SprinklerPlacement>();

            // Generate sprinkler grid
            var gridResult = ceilingGeometryService.GenerateGrid(p1, p2, p3, p4, margin, spacing);

            // Find connections for each sprinkler
            foreach (var sPos in gridResult.Points)
            {
                var (connectionPoint, pipeID) = ceilingGeometryService.GetNearestPipe(sPos, waterPipes);
                results.Add(new SprinklerPlacement
                {
                    Position = sPos,
                    ConnectionPoint = connectionPoint,
                    PipeID = pipeID
                });
            }

            return new SprinklerLayoutResult(gridResult.NumberOfSprinklers, results);
        }
    }
}
