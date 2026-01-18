namespace SprinklerSystem.Core.Models
{
    public class SprinklerLayoutResult(int numberOfSprinklers, List<SprinklerPlacement> placements)
    {
        public int NumberOfSprinklers { get; init; } = numberOfSprinklers;
        public List<SprinklerPlacement> Placements { get; init; } = placements;
    }
}
