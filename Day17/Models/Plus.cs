using Shared;

namespace Day17.Models
{
    public class Plus : Rock
    {
        public override string Name => "Plus";
        public Plus(long highestRockRow)
        {
            Points = new List<Coordinates>()
            {
                new (highestRockRow + 4, 3),
                new (highestRockRow + 5, 2),
                new (highestRockRow + 5, 3),
                new (highestRockRow + 5, 4),
                new (highestRockRow + 6, 3),
            };
        }
    }
}
