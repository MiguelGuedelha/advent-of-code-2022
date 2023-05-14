using Shared;

namespace Day17.Models
{
    public class Corner : Rock
    {
        public override string Name => "Corner";

        public Corner(long highestRockRow)
        {
            Points = new List<Coordinates>
            {
                new (highestRockRow+4, 2),
                new (highestRockRow+4, 3),
                new (highestRockRow+4, 4),
                new (highestRockRow+5, 4),
                new (highestRockRow+6, 4),
            };
        }
    }
}
