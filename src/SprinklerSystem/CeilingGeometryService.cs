namespace SprinklerSystem
{
    public class CeilingGeometryService
    {
        public List<Point> GenerateGrid(Point p1, Point p2, Point p3, Point p4, double margin, double spacing)
        {
            var points = new List<Point>();

            // Calculate edge lengths
            double len_12 = Point.Distance(p1, p2);
            double len_14 = Point.Distance(p1, p4);

            // Calculate unit direction vectors
            double unit_12_x = (p2.X - p1.X) / len_12;
            double unit_12_y = (p2.Y - p1.Y) / len_12;
            double unit_14_x = (p4.X - p1.X) / len_14;
            double unit_14_y = (p4.Y - p1.Y) / len_14;

            // Calculate sprinkler counts
            double available_12 = len_12 - 2 * margin;
            double available_14 = len_14 - 2 * margin;

            int count_12 = (int)Math.Floor(available_12 / spacing) + 1;
            int count_14 = (int)Math.Floor(available_14 / spacing) + 1;

            for (int i = 0; i < count_14; i++)
            {
                for (int j = 0; j < count_12; j++)
                {
                    double dist_along_12 = margin + j * spacing;
                    double dist_along_14 = margin + i * spacing;

                    // Calculate X, Y using vector addition
                    double x = p1.X + dist_along_12 * unit_12_x + dist_along_14 * unit_14_x;
                    double y = p1.Y + dist_along_12 * unit_12_y + dist_along_14 * unit_14_y;

                    // Bilinear interpolation for Z coordinate
                    double u = dist_along_12 / len_12;
                    double v = dist_along_14 / len_14;
                    double z = (1 - u) * (1 - v) * p1.Z + u * (1 - v) * p2.Z + u * v * p3.Z + (1 - u) * v * p4.Z;

                    points.Add(new Point(x, y, z));
                }
            }

            return points;
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
