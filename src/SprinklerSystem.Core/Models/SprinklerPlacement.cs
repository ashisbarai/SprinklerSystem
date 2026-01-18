namespace SprinklerSystem.Core.Models
{
    public class SprinklerPlacement
    {
        public Point Position { get; set; }
        public Point ConnectionPoint { get; set; }
        public string PipeID { get; set; }
    }
}
