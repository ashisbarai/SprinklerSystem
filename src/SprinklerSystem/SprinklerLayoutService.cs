namespace SprinklerSystem
{
    public class SprinklerLayoutService
    {
        private readonly CeilingGeometryService _ceilingGeometryService;

        public SprinklerLayoutService(CeilingGeometryService ceilingGeometryService)
        {
            _ceilingGeometryService = ceilingGeometryService;
        }

        public List<SprinklerPlacement> GetPlacements(Point p1, Point p2, Point p3, Point p4, List<WaterPipe> waterPipes, double margin, double spacing)
        {
            var results = new List<SprinklerPlacement>();

            // Generate sprinkler grid
            var sprinklerPositions = _ceilingGeometryService.GenerateGrid(p1, p2, p3, p4, margin, spacing);

            // Find connections for each sprinkler
            foreach (var sPos in sprinklerPositions)
            {
                var (connectionPoint, pipeID) = _ceilingGeometryService.GetNearestPipe(sPos, waterPipes);
                results.Add(new SprinklerPlacement
                {
                    Position = sPos,
                    ConnectionPoint = connectionPoint,
                    PipeID = pipeID
                });
            }

            return results;
        }
    }
}
