using System.Text;
using var reader = new StreamReader("input.txt", Encoding.UTF8);
var inputString = reader.ReadToEnd();

var initialSteps = inputString.Split(Environment.NewLine).Select(x => new { Direction = x.Split(" ")[0], Amount = int.Parse(x.Split(" ")[1]) });

var individualSteps = initialSteps.SelectMany(x => Enumerable.Range(0, x.Amount).Select(y => x.Direction)).ToList();

var visitedCoordinates = new HashSet<Coordinates>();

var knots = Enumerable.Range(0, 10).Select(x => new Coordinates(0, 0)).ToList();

visitedCoordinates.Add(knots[^1] with { });

var moves = new Dictionary<string, (int, int)>
{
    { "U", (0, 1) },
    { "R", (1, 0) },
    { "D", (0, -1) },
    { "L", (-1, 0) }
};

var diagonalMoves = new List<(int, int)>
{
    (1, 1),
    (-1, -1),
    (1, -1),
    (-1, 1)
};

foreach (var step in individualSteps)
{

    var move = moves[step];
    knots[0] = new Coordinates(knots[0].X + move.Item1, knots[0].Y + move.Item2);
    for (var i = 1; i < knots.Count; i++)
    {
        if (!IsAdjacent(knots[i - 1], knots[i]))
        {
            if (knots[i].X == knots[i - 1].X || knots[i].Y == knots[i - 1].Y)
            {
                foreach (var tempMove in moves.Values)
                {
                    var tempKnot = new Coordinates(knots[i].X + tempMove.Item1, knots[i].Y + tempMove.Item2);
                    if (IsAdjacent(knots[i - 1], tempKnot))
                    {
                        knots[i] = tempKnot;
                        break;
                    }
                }
            }
            else
            {
                foreach (var diagonalMove in diagonalMoves)
                {

                    var tempKnot = new Coordinates(knots[i].X + diagonalMove.Item1, knots[i].Y + diagonalMove.Item2);
                    if (IsAdjacent(knots[i - 1], tempKnot))
                    {
                        knots[i] = tempKnot;
                        break;
                    }
                }
            }
        }
    }

    visitedCoordinates.Add(knots[^1] with { });
}

Console.WriteLine(visitedCoordinates.Count);
Console.ReadLine();

bool IsAdjacent(Coordinates p1, Coordinates p2)
{
    var yDist = Math.Abs(p1.Y - p2.Y);
    var xDist = Math.Abs(p1.X - p2.X);

    return xDist < 2 && yDist < 2;
}

record Coordinates(int X, int Y);