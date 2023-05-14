using Day19;
using System.Text;
using System.Text.RegularExpressions;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var inputString = reader.ReadToEnd();

var regex = new Regex(@"^Blueprint (\d+).+ore robot costs (\d+).+clay robot costs (\d+).+obsidian robot costs (\d+).+and (\d+).+geode robot costs (\d+).+and (\d+).+$");
var blueprintLines = inputString.Split(Environment.NewLine);

var blueprints = blueprintLines
    .Select(x => regex.Match(x))
    .Select(x => new
    {
        Name = int.Parse(x.Groups[1].Value),
        Blueprint = new Blueprint(
        new Cost(int.Parse(x.Groups[2].Value)),
        new Cost(int.Parse(x.Groups[3].Value)),
        new Cost(int.Parse(x.Groups[4].Value), int.Parse(x.Groups[5].Value)),
        new Cost(int.Parse(x.Groups[6].Value), 0, int.Parse(x.Groups[7].Value)))
    })
    .ToDictionary(x => x.Name, x => x.Blueprint);

Console.ReadLine();