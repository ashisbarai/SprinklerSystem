namespace SprinklerSystem
{
    public class WaterPipe
    {
        public string ID { get; init; }
        public Point Start { get; init; }
        public Point End { get; init; }

        public WaterPipe(string id, Point start, Point end)
        {
            ID = id;
            Start = start;
            End = end;
        }
    }
}
