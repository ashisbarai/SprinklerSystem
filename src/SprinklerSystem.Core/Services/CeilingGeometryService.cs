using SprinklerSystem.Core.Abstractions;
using SprinklerSystem.Core.Models;

namespace SprinklerSystem.Core.Services
{
    public class CeilingGeometryService : ICeilingGeometryService
    {
        public GridResult GenerateGrid(Point p1, Point p2, Point p3, Point p4, double margin, double spacing)
        {
            var points = new List<Point>();

            // Calculate edge lengths
            double len12 = Point.Distance(p1, p2);
            double len14 = Point.Distance(p1, p4);

            // Calculate unit direction vectors
            double unit12X = (p2.X - p1.X) / len12;
            double unit12Y = (p2.Y - p1.Y) / len12;
            double unit14X = (p4.X - p1.X) / len14;
            double unit14Y = (p4.Y - p1.Y) / len14;

            // Calculate sprinkler counts
            double available12 = len12 - 2 * margin;
            double available14 = len14 - 2 * margin;

            int count12 = (int)Math.Floor(available12 / spacing) + 1;
            int count14 = (int)Math.Floor(available14 / spacing) + 1;

            int numberOfSprinklers = count12 * count14;

            for (int i = 0; i < count14; i++)
            {
                for (int j = 0; j < count12; j++)
                {
                    double distAlong12 = margin + j * spacing;
                    double distAlong14 = margin + i * spacing;

                    // Calculate X, Y using vector addition
                    double x = p1.X + distAlong12 * unit12X + distAlong14 * unit14X;
                    double y = p1.Y + distAlong12 * unit12Y + distAlong14 * unit14Y;

                    // Bilinear interpolation for Z coordinate
                    double u = distAlong12 / len12;
                    double v = distAlong14 / len14;
                    double z = (1 - u) * (1 - v) * p1.Z + u * (1 - v) * p2.Z + u * v * p3.Z + (1 - u) * v * p4.Z;

                    points.Add(new Point(x, y, z));
                }
            }

            return new GridResult(numberOfSprinklers, points);
        }

        public Point GetConnectionPoint(Point sprinkler, WaterPipe pipe)
        {
            // Vector AB (pipe direction)
            double vX = pipe.End.X - pipe.Start.X;
            double vY = pipe.End.Y - pipe.Start.Y;
            double vZ = pipe.End.Z - pipe.Start.Z;

            // Vector AP (pipe start to sprinkler)
            double wX = sprinkler.X - pipe.Start.X;
            double wY = sprinkler.Y - pipe.Start.Y;
            double wZ = sprinkler.Z - pipe.Start.Z;

            // Project: t = (AP · AB) / (AB · AB)
            double dotWV = wX * vX + wY * vY + wZ * vZ;
            double dotVV = vX * vX + vY * vY + vZ * vZ;

            // t is the projection factor (0.0 to 1.0)
            double t = Math.Clamp(dotWV / dotVV, 0.0, 1.0);

            return new Point(
                pipe.Start.X + t * vX,
                pipe.Start.Y + t * vY,
                pipe.Start.Z + t * vZ
            );
        }

        public (Point connectionPoint, string pipeID) GetNearestPipe(Point sprinkler, List<WaterPipe> pipes)
        {
            string bestPipeID = "";
            Point bestConn = default;
            double minDistance = double.MaxValue;

            foreach (var pipe in pipes)
            {
                Point conn = GetConnectionPoint(sprinkler, pipe);
                double d = Point.Distance(sprinkler, conn);

                if (d < minDistance)
                {
                    minDistance = d;
                    bestPipeID = pipe.ID;
                    bestConn = conn;
                }
            }

            return (bestConn, bestPipeID);
        }
    }
}
