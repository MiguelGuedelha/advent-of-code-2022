using Shared;
using System.Text;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var inputString = reader.ReadToEnd();

var coords = inputString
    .Split(Environment.NewLine)
    .Select(x => x.Split(','))
    .Select(x => new Coordinates3D(int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2])))
    .ToHashSet();

var vectors = new List<Coordinates3D>
{
    new (1,0,0),
    new (-1,0,0),
    new (0,1,0),
    new (0,-1,0),
    new (0,0,-1),
    new (0,0,1),
};

var cubesAndSides = coords
    .Select(x => new { Cube = x, Sides = GenerateAdjacentCubes(x, vectors) })
    .ToList();

//Part 1
//foreach (var cubeAndSides in cubesAndSides)
//{
//    cubeAndSides.Sides.RemoveAll(x => coords.Contains(x));
//}

//var exposedSides = cubesAndSides.Select(x => x.Sides).SelectMany(x => x).Count();
//Console.WriteLine(exposedSidesP1);

//Part 2
var minX = coords.MinBy(x => x.X)!.X;
var maxX = coords.MaxBy(x => x.X)!.X;
var minY = coords.MinBy(x => x.Y)!.Y;
var maxY = coords.MaxBy(x => x.Y)!.Y;
var minZ = coords.MinBy(x => x.Z)!.Z;
var maxZ = coords.MaxBy(x => x.Z)!.Z;

foreach (var cubeAndSides in cubesAndSides)
{
    cubeAndSides.Sides.RemoveAll(x => coords.Contains(x));
}

var exposedSides = cubesAndSides
    .Select(x => x.Sides)
    .SelectMany(x => x)
    .ToList();

var visited = new HashSet<Coordinates3D>();

var start = new Coordinates3D(minX - 1, minY - 1, minZ - 1);

var stack = new Queue<Coordinates3D>();

stack.Enqueue(start);

while (stack.Any())
{
    var current = stack.Dequeue();
    foreach (var adjacent in GenerateAdjacentCubes(current, vectors))
    {
        if (IsOutOfBounds(adjacent) || coords.Contains(adjacent))
        {
            continue;
        }

        if (!visited.Contains(adjacent))
        {
            visited.Add(adjacent);
            stack.Enqueue(adjacent);
        }
    }
}

var result = exposedSides.Where(x => visited.Contains(x)).ToList().Count;
Console.WriteLine(result);

Console.ReadLine();

List<Coordinates3D> GenerateAdjacentCubes(Coordinates3D cube, List<Coordinates3D> generationVectors)
{
    return generationVectors
        .Select(x => new Coordinates3D(cube.X + x.X, cube.Y + x.Y, cube.Z + x.Z))
        .ToList();
}

bool IsOutOfBounds(Coordinates3D cube)
{
    return cube.X < minX - 1 ||
           cube.X > maxX + 1 ||
           cube.Y < minY - 1 ||
           cube.Y > maxY + 1 ||
           cube.Z < minZ - 1 ||
           cube.Z > maxZ + 1;
}