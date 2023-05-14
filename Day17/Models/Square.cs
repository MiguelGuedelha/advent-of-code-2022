using Shared;

namespace Day17.Models
{
    public class Square : Rock
    {
        public override string Name => "Square";
        public Square(long highestRockRow)
        {
            Points = new List<Coordinates>
            {
                new (highestRockRow+4, 2),
                new (highestRockRow+4, 3),
                new (highestRockRow+5, 2),
                new (highestRockRow+5, 3)
            };
        }
    }
}
