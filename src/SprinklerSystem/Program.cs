using SprinklerSystem;

// Room ceiling corner coordinates (clockwise: P1, P2, P3, P4)
Point p1 = new Point(97500.01, 34000.00, 2500.00);
Point p2 = new Point(85647.67, 43193.61, 2500.00);
Point p3 = new Point(91776.75, 51095.16, 2530.00);
Point p4 = new Point(103629.07, 41901.55, 2530.00);

// Water pipes
var waterPipes = new List<WaterPipe>
{
    new WaterPipe("P1", new Point(98242.11, 36588.29, 3000.00), new Point(87970.10, 44556.09, 3500.00)),
    new WaterPipe("P2", new Point(99774.38, 38563.68, 3500.00), new Point(89502.37, 46531.47, 3000.00)),
    new WaterPipe("P3", new Point(101306.65, 40539.07, 3000.00), new Point(91034.63, 48507.01, 3000.00))
};

// Sprinkler parameters
double margin = 2500.0;
double spacing = 2500.0;

var ceilingGeometryService = new CeilingGeometryService();
// Calculate
var placements = new SprinklerLayoutService(ceilingGeometryService).GetPlacements(p1, p2, p3, p4, waterPipes, margin, spacing);

// Print sprinkler count
Console.WriteLine($"Number of Sprinklers: {placements.Count}");
Console.WriteLine();

// Print table header
Console.WriteLine("| #  | Sprinkler Position (X, Y, Z)        | Connection Point (X, Y, Z)          |");
Console.WriteLine("|----|-------------------------------------|-------------------------------------|");

// Print sprinkler positions in tabular format
int index = 1;
foreach (var sprinkler in placements)
{
    Console.WriteLine($"| {index,-2} | {sprinkler.Position,-35} | {sprinkler.ConnectionPoint,-35} |");
    index++;
}
