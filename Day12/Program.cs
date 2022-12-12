using System.Text;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var inputString = reader.ReadToEnd().Split(Environment.NewLine).Select(x => x.ToCharArray()).ToArray();

var nodes = new Dictionary<(int, int), Node>();

for (int i = 0; i < inputString.Length; i++)
{
    for (int j = 0; j < inputString[0].Length; j++)
    {
        nodes.Add((i, j), new Node
        {
            Character = inputString[i][j],
            AdjacentNodes = new List<Node>(),
            Column = j,
            Row = i,
            Explored = false
        });
    }
}

var adjacentMovements = new List<(int, int)>()
{
    (1, 0),
    (-1, 0),
    (0, 1),
    (0, -1),
};

foreach (var node in nodes.Values)
{
    foreach (var movement in adjacentMovements)
    {
        var hasNode = nodes.TryGetValue((node.Row + movement.Item1, node.Column + movement.Item2), out var tempNode);
        if (hasNode)
        {
            node.AdjacentNodes.Add(tempNode);
        }
    }
}

var end = nodes.Values.FirstOrDefault(x => x.Character == 'E');

var startingPoints = nodes.Values.Where(x => x.Elevation == 'a').ToList();

var solution = new List<Node>();

foreach (var item in startingPoints)
{
    ClearExplored();
    var tempSolution = Solution(item, end);
    if (solution.Count == 0 || solution.Count > tempSolution.Count && tempSolution.Count > 0)
    {
        solution = tempSolution;
    }
}

var solutionGrid = new char[inputString.Length, inputString[0].Length];
for (int i = 0; i < inputString.Length; i++)
{
    for (int j = 0; j < inputString[0].Length; j++)
    {
        solutionGrid[i, j] = '.';
    }
}

solutionGrid[end.Row, end.Column] = 'E';

for (int i = 0; i < solution.Count; i++)
{
    var next = i + 1 < solution.Count ? solution[i + 1] : end;
    var curr = solution[i];

    var character = '.';
    if (curr.Row > next.Row)
    {
        character = '^';
    }

    if (curr.Row < next.Row)
    {
        character = 'v';
    }

    if (curr.Column > next.Column)
    {
        character = '<';
    }

    if (curr.Column < next.Column)
    {
        character = '>';
    }

    solutionGrid[curr.Row, curr.Column] = character;
}

for (int i = 0; i < inputString.Length; i++)
{
    for (int j = 0; j < inputString[0].Length; j++)
    {
        Console.Write(solutionGrid[i, j]);
    }
    Console.WriteLine();
}

Console.WriteLine(solution.Count);
Console.ReadLine();

void ClearExplored()
{
    foreach (var node in nodes.Values)
    {
        node.Parent = null;
        node.Explored = false;
    }
}

static List<Node> Solution(Node start, Node end)
{

    var queue = new Queue<Node>();
    start.Explored = true;

    queue.Enqueue(start);

    while (queue.Any())
    {
        var current = queue.Dequeue();
        if (current.Character == 'E')
        {
            end = current;
            break;
        }
        foreach (var node in current.AdjacentNodes)
        {
            if (!node.Explored)
            {
                if (node.Elevation - current.Elevation < 2)
                {
                    node.Explored = true;
                    node.Parent = current;
                    queue.Enqueue(node);
                }
            }

        }
    }

    return GetSolutionPath(start, end!).ToList();
}

static List<Node> GetSolutionPath(Node start, Node end)
{

    List<Node> nodes = new List<Node>();
    var current = end;
    while (current.Parent != null)
    {
        nodes.Add(current.Parent);
        current = current.Parent;
    }
    nodes.Reverse();
    return nodes;
}

record Node
{
    public List<Node> AdjacentNodes { get; set; }

    public Node? Parent { get; set; }

    public int Column { get; init; }

    public int Row { get; init; }

    public bool Explored { get; set; }

    public char Character { get; init; }

    public int Elevation => Character switch
    {
        'S' => 'a',
        'E' => 'z',
        _ => Character
    };
}