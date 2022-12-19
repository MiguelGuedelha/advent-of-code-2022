using Shared;

namespace Day17.Models
{
    public class VerticalLine : Rock
    {
        public override string Name => "VerticalLine";
        public VerticalLine(long highestRockRow)
        {
            Points = new List<Coordinates>
            {
                new(highestRockRow + 4, 2),
                new(highestRockRow + 5, 2),
                new(highestRockRow + 6, 2),
                new(highestRockRow + 7, 2),
            };
        }
    }
}
