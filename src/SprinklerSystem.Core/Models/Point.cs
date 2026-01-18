namespace SprinklerSystem.Core.Models
{
    public readonly record struct Point(double X, double Y, double Z)
    {
        public static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) +
                           Math.Pow(p2.Y - p1.Y, 2) +
                           Math.Pow(p2.Z - p1.Z, 2));
        }

        public override string ToString() => $"({X:F2}, {Y:F2}, {Z:F2})";
    }
}
