using System.Text;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var inputString = reader.ReadToEnd();
var lines = inputString
    .Split(Environment.NewLine)
    .Select(x => x.Split(" "))
    .ToList();

var graph = new Dictionary<string, HashSet<string>>();
var valves = new Dictionary<string, Valve>();

lines.ForEach(x =>
{
    var valveName = x[1];
    var valveFlowRate = int.Parse(x[4].Split("=")[1].Replace(";", ""));
    var valvesStartIndex =
        (Array.IndexOf(x, "valves") == -1 ? Array.IndexOf(x, "valve") : Array.IndexOf(x, "valves")) + 1;
    var valveTunnels = x[valvesStartIndex..]
        .Select(y => y.Replace(",", ""))
        .ToHashSet();

    graph[valveName] = valveTunnels;

    if (valveFlowRate > 0)
    {
        valves.Add(valveName, new Valve(valveFlowRate));
    }

});

var distanceMatrix = GenerateDistanceMatrix(graph);


Console.ReadLine();

Dictionary<(string, string), int> GenerateDistanceMatrix(Dictionary<string, HashSet<string>> _graph)
{
    var _distanceMatrix = new Dictionary<(string, string), int>();

    var pairs = _graph.Keys
        .SelectMany(x => _graph.Keys.Select(y => (x, y)))
        .ToList();

    foreach (var pair in pairs)
    {
        _distanceMatrix[pair] = 99999;
    }

    foreach (var (key, neighbors) in _graph)
    {
        foreach (var neighbor in neighbors)
        {
            _distanceMatrix[(key, neighbor)] = 1;
        }

        _distanceMatrix[(key, key)] = 0;
    }

    foreach (var i in _graph.Keys)
    {
        foreach (var j in _graph.Keys)
        {
            foreach (var k in _graph.Keys)
            {
                if (_distanceMatrix[(i, j)] > _distanceMatrix[(i, k)] + _distanceMatrix[(k, j)])
                {
                    _distanceMatrix[(i, j)] = _distanceMatrix[(i, k)] + _distanceMatrix[(k, j)];
                }
            }
        }
    }

    foreach (var graphKey in _graph.Keys)
    {
        _distanceMatrix.Remove((graphKey, graphKey));
        if (!valves.ContainsKey(graphKey))
        {
            foreach (var otherKey in graph.Keys.Where(x => !x.Equals(graphKey)))
            {
                _distanceMatrix.Remove((graphKey, otherKey));
            }
        }

        _distanceMatrix
            .Where(x => x.Value == 99999)
            .Select(x => x.Key);
    }

    return _distanceMatrix;
}

record Valve(int FlowRate);