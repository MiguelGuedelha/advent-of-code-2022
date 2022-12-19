using System.Text;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var inputString = reader.ReadToEnd();
var lines = inputString.Split(Environment.NewLine).Select(x => x.Split(" ")).ToList();

var valves = new Dictionary<string, Valve>();

var TRAVEL_TIME = 1;
var TURN_ON_TIME = 1;
var timeLeft = 30;

lines.ForEach(x =>
{
    var valveName = x[1];
    var valvePressure = int.Parse(x[4].Split("=")[1].Replace(";", ""));
    var valvesStartIndex = Array.IndexOf(x, "valves") + 1;
    var valveTunnels = x[valvesStartIndex..]
        .Select(y => y.Replace(",", ""))
        .ToHashSet();

    valves[valveName] = new Valve(valvePressure, valveTunnels);
});

var current = valves["AA"];
for (var i = 30; i > 0; i--)
{
    var options = current.Tunnels.Select(x => valves[x]).ToList();
}

Console.ReadLine();

record Valve(int Pressure, HashSet<string> Tunnels);