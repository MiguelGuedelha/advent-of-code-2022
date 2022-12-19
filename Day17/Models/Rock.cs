using Shared;

namespace Day17.Models
{
    public abstract class Rock
    {
        public List<Coordinates> Points = null!;
        public abstract string Name { get; }

        public bool Push(char direction, HashSet<Coordinates> space)
        {
            return direction switch
            {
                '>' => Push(1, space),
                '<' => Push(-1, space),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private bool Push(int push, HashSet<Coordinates> space)
        {
            var newPos = Points
                .Select(x => x with { Column = x.Column + push })
                .ToList();

            if (Collides(newPos, space))
            {
                return false;
            }

            if (newPos.All(x => x.Column is >= 0 and <= 6))
            {
                Points = newPos;
            }

            return true;
        }

        public bool Fall(HashSet<Coordinates> space)
        {
            var newPos = Points.Select(x => x with { Row = x.Row - 1 }).ToList();

            if (newPos.Any(x => x.Row <= 0) || Collides(newPos, space))
            {
                return false;
            }

            Points = newPos;
            return true;
        }

        public bool Collides(List<Coordinates> points, HashSet<Coordinates> space)
        {
            return points.Any(space.Contains);
        }
    }
}
