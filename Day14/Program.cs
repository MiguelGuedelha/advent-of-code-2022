using Shared;
using System.Text;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var inputString = reader.ReadToEnd();
var maxRow = 0;
var maxColumn = 0;
var sandOrigin = new Coordinates(0, 500);
var paths = inputString
    .Split(Environment.NewLine)
    .Select(line => line
        .Split(" -> ")
        .Select(step => step
            .Split(",")
        )
        .Select(coords =>
        {
            var column = int.Parse(coords[0]);
            var row = int.Parse(coords[1]);
            maxRow = maxRow < row ? row : maxRow;
            maxColumn = maxColumn < column ? column : maxColumn;
            return new Coordinates(row, column);
        })
        .ToList()
    )
    .ToList();

var space = GenerateSpace(maxRow + 1, maxColumn + 1, sandOrigin, paths);
var dictSpace = GenerateDictSpace(sandOrigin, paths);

bool clogged = false;
int sandCount = 0;

var moves = new List<(int, int)>
{
    (1, 0),
    (1, -1),
    (1,1)
};

while (!clogged)
{
    var sandGrain = sandOrigin with { };
    var isAtRest = false;
    while (!isAtRest)
    {
        var tempSandGrain = sandGrain;
        foreach (var move in moves)
        {
            var newRow = sandGrain.Row + move.Item1;
            var newColumn = sandGrain.Column + move.Item2;

            if (newRow == maxRow + 2)
            {
                break;
            }

            var isAvailable = !dictSpace.TryGetValue(new Coordinates(newRow, newColumn), out var positionChar);
            if (isAvailable)
            {
                tempSandGrain = new Coordinates(newRow, newColumn);
                break;
            }
        }

        if (tempSandGrain == sandGrain)
        {
            sandCount++;
            isAtRest = true;
            //space[sandGrain.Row, sandGrain.Column] = 'o';
            dictSpace[tempSandGrain] = 'o';
            if (tempSandGrain == sandOrigin)
            {
                clogged = true;
            }
        }
        else
        {
            sandGrain = tempSandGrain;
        }
    }
}

Console.WriteLine(sandCount);
Console.ReadLine();

static void DrawRockFormation(Coordinates? prev, Coordinates current, char[,] space)
{
    if (prev == null)
    {
        space[current.Row, current.Column] = '#';
        return;
    }

    if (prev.Row != current.Row)
    {
        var startingRow = Math.Min(prev.Row, current.Row);
        var endRow = Math.Max(prev.Row, current.Row);
        for (var i = startingRow; i <= endRow; i++)
        {
            space[i, current.Column] = '#';
        }
        return;
    }

    var startingColumn = Math.Min(prev.Column, current.Column);
    var endColumn = Math.Max(prev.Column, current.Column);
    for (var i = startingColumn; i <= endColumn; i++)
    {
        space[current.Row, i] = '#';
    }

}

static char[,] GenerateSpace(int spaceRows, int spaceColumns, Coordinates origin, List<List<Coordinates>> rockPaths)
{
    var spaceGrid = new char[spaceRows, spaceColumns];
    for (int i = 0; i < spaceRows; i++)
    {
        for (int j = 0; j < spaceColumns; j++)
        {
            spaceGrid[i, j] = '.';
        }
    }

    spaceGrid[origin.Row, origin.Column] = '+';

    foreach (var path in rockPaths)
    {
        Coordinates? prevCoords = null;
        for (var i = 0; i < path.Count; i++)
        {
            if (i > 0)
            {
                prevCoords = path[i - 1];
            }

            DrawRockFormation(prevCoords, path[i], spaceGrid);
        }
    }

    return spaceGrid;
}

static Dictionary<Coordinates, char> GenerateDictSpace(
    Coordinates origin,
    List<List<Coordinates>> rockPaths)
{
    var dictSpace = new Dictionary<Coordinates, char>();
    dictSpace.Add(origin, '+');

    foreach (var path in rockPaths)
    {
        Coordinates? prevCoords = null;
        for (var i = 0; i < path.Count; i++)
        {
            if (i > 0)
            {
                prevCoords = path[i - 1];
            }

            AddRockFormation(prevCoords, path[i], dictSpace);
        }
    }

    return dictSpace;
}

static void AddRockFormation(Coordinates? prev, Coordinates current, Dictionary<Coordinates, char> space)
{
    if (prev == null)
    {
        space[current] = '#';
        return;
    }

    if (prev.Row != current.Row)
    {
        var startingRow = Math.Min(prev.Row, current.Row);
        var endRow = Math.Max(prev.Row, current.Row);
        for (var i = startingRow; i <= endRow; i++)
        {
            space[current with { Row = i }] = '#';
        }
        return;
    }

    var startingColumn = Math.Min(prev.Column, current.Column);
    var endColumn = Math.Max(prev.Column, current.Column);
    for (var i = startingColumn; i <= endColumn; i++)
    {
        space[current with { Column = i }] = '#';
    }
}