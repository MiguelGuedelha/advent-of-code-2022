using Day17.Models;
using Shared;
using System.Text;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var inputString = reader.ReadToEnd();
var jets = inputString.ToCharArray();
var space = new HashSet<Coordinates>();

//PART 1
//var t = 0;
//for (int i = 0; i < 2022; i++)
//{
//    var resting = false;
//    var rock = GenerateRock(i, space);
//    while (!resting)
//    {
//        var direction = jets[t++ % jets.Length];
//        rock.Push(direction, space);
//        resting = !rock.Fall(space);
//    }

//    rock.Points.ForEach(x => space.Add(x));
//}

//var maxHeight = space.MaxBy(x => x.Row)!.Row;

//Console.WriteLine(maxHeight);
//Console.ReadLine();

//PART 2
//Need to detect a cycle on the tower structure
var index = 0L;
var t = 0L;
var stepsState = new List<(long, string, string)>();
var heightState = new List<long>();
var cycleStart = 0;
var totalRocks = 1000000000000;

while (true)
{
    var resting = false;
    var rock = GenerateRock(index, space);
    while (!resting)
    {
        var direction = jets[t++ % jets.Length];
        rock.Push(direction, space);
        resting = !rock.Fall(space);
    }

    rock.Points.ForEach(x => space.Add(x));
    var line = TowerLine(space);

    var prevT = t - 1;

    var toInsert = (prevT % jets.Length, rock.Name, line);

    if (stepsState.Contains(toInsert))
    {
        rock.Points.ForEach(x => space.Remove(x));
        cycleStart = stepsState.IndexOf(toInsert);
        break;
    }

    stepsState.Add(toInsert);
    heightState.Add(GetTowerHeight(space));

    index++;
}

var start = stepsState.Take(cycleStart).ToList();
var startHeights = heightState.Take(cycleStart).ToList();
var cycle = stepsState.Skip(cycleStart).ToList();
var cycleHeights = heightState.Skip(cycleStart).ToList();

var timesRepeated = (totalRocks - start.Count) / cycle.Count;
var leftOverRocks = totalRocks - start.Count - (cycle.Count * timesRepeated);

var cycledHeight = startHeights[^1] + (cycleHeights[^1] - startHeights[^1]) * timesRepeated;
var leftOverHeight = 0L;

if (leftOverRocks > 0)
{
    leftOverHeight = cycleHeights[(int)leftOverRocks - 1] - startHeights[^1];
}

cycledHeight += leftOverHeight;

Console.WriteLine(cycledHeight);
Console.ReadLine();


Rock GenerateRock(long i, IReadOnlySet<Coordinates>? gameSpace)
{
    var highestRow = gameSpace?.MaxBy(x => x.Row)?.Row ?? 0;
    return (i % 5) switch
    {
        0 => new HorizontalLine(highestRow),
        1 => new Plus(highestRow),
        2 => new Corner(highestRow),
        3 => new VerticalLine(highestRow),
        4 => new Square(highestRow),
        _ => throw new NotImplementedException() //Not possible anyways
    };
}

void DrawTunnel(long height, IReadOnlySet<Coordinates> rockCoords)
{
    for (var i = height + 5; i > 0; i--)
    {
        Console.Write("|");
        for (var j = 0; j < 7; j++)
        {
            if (rockCoords.Contains(new Coordinates(i, j)))
            {
                Console.Write("#");
                continue;
            }

            Console.Write(".");
        }
        Console.Write("|");
        Console.WriteLine();
    }

    Console.Write("+");
    for (var i = 0; i < 7; i++)
    {
        Console.Write("-");
    }
    Console.Write("+");
    Console.WriteLine();
}

string TowerLine(IReadOnlySet<Coordinates> rockCoords)
{
    var sb = new StringBuilder();
    var height = GetTowerHeight(rockCoords);

    //Arbitrarily big enough number to detect a repeated pattern in the tower structure
    //Trial and error
    for (var i = height; i >= height - 100; i--)
    {
        for (var j = 0L; j < 7; j++)
        {
            if (i >= 1)
            {
                sb.Append(rockCoords.Contains(new Coordinates(i, j)) ? "#" : ".");
            }
        }
    }
    return sb.ToString();
}

HashSet<Coordinates> SpaceTopBorder(HashSet<Coordinates> coordsSpace)
{
    var border = new HashSet<Coordinates>();
    for (var i = 0; i < 7; i++)
    {
        border.Add(coordsSpace.Where(x => x.Column == i).MaxBy(x => x.Row) ?? new Coordinates(0, i));
    }

    return border;
}

long GetTowerHeight(IReadOnlySet<Coordinates> rockCoords)
{
    return rockCoords.MaxBy(x => x.Row)?.Row ?? 0;
}

long GetTowerBottom(IReadOnlySet<Coordinates> rockCoords)
{
    return rockCoords.MinBy(x => x.Row)?.Row ?? 0;
}