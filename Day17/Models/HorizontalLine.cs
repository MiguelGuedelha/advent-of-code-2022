using Shared;

namespace Day17.Models
{
    public class HorizontalLine : Rock
    {
        public override string Name => "HorizontalLine";
        public HorizontalLine(long highestRockRow)
        {
            Points = new List<Coordinates>
            {
                new (highestRockRow+4, 2),
                new (highestRockRow+4, 3),
                new (highestRockRow+4, 4),
                new (highestRockRow+4, 5),
            };
        }
    }
}
