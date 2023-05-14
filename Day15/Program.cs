using Shared;
using System.Text;
using System.Text.RegularExpressions;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var regex = new Regex(@"x=([-]*\d+).+y=([-]*\d+).+x=([-]*\d+).+y=([-]*\d+)");
var inputString = reader.ReadToEnd();
var UPPER_LIMIT = 4000000L;

var lines = inputString.Split(Environment.NewLine);

var sensorsBeacons = lines.Select(line =>
{
    var match = regex.Match(line);

    var sensor = new Coordinates(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[1].Value));
    var beacon = new Coordinates(int.Parse(match.Groups[4].Value), int.Parse(match.Groups[3].Value));
    var maxDistance = Math.Abs(sensor.Row - beacon.Row) + Math.Abs(sensor.Column - beacon.Column);

    return (beacon, sensor, maxDistance);
}).ToList();

long result = 1;

for (int i = 0; i <= UPPER_LIMIT; i++)
{
    var ranges = new HashSet<(long, long)>();
    foreach (var (_, sensor, maxDistance) in sensorsBeacons)
    {
        var distanceFromSensor = Math.Abs(i - sensor.Row);
        if (distanceFromSensor <= maxDistance)
        {
            var leftoverDist = maxDistance - distanceFromSensor;
            var leftLimit = Math.Max(sensor.Column - leftoverDist, 0);
            var rightLimit = Math.Min(sensor.Column + leftoverDist, 4000000);
            ranges.Add((leftLimit, rightLimit));
        }
    }

    var (isFilled, point) = IsFilled(ranges);
    if (!isFilled)
    {
        result = point!.Value * UPPER_LIMIT + i;
        break;
    }
}

Console.WriteLine(result);
Console.ReadLine();

(bool, long?) IsFilled(HashSet<(long, long)> data)
{
    if (data.Any(x => x.Item1 == 0 && x.Item2 == UPPER_LIMIT))
    {
        return (true, null);
    }

    var line = new HashSet<(long, long)>();
    foreach (var (start, end) in data)
    {
        if (!line.Any())
        {
            line.Add((start, end));
            continue;
        }

        var rangeToExtend = line.Where(x => Math.Max(start, x.Item1) - Math.Min(end, x.Item2) <= 0).ToList();

        if (rangeToExtend.Any())
        {
            line.RemoveWhere(x => rangeToExtend.Contains(x));
            var mergedRange = rangeToExtend.Aggregate((start, end), (total, current) =>
                (Math.Min(total.start, current.Item1), Math.Max(total.end, current.Item2)));
            line.Add(mergedRange);
        }
        else
        {
            line.Add((start, end));
        }
    }

    if (line.Count > 1)
    {
        var values = line.OrderBy(x => x.Item1).ToList();
        return (false, values[0].Item2 + 1);
    }

    return (true, null);
}

//var space = new Dictionary<Coordinates, char>();

//var rowToCheck = 2000000;

//foreach (var sensorBeacon in sensorsBeacons)
//{
//    space[sensorBeacon.sensor] = 'S';
//    space[sensorBeacon.beacon] = 'B';

//    var dRow = Math.Abs(sensorBeacon.beacon.Row - sensorBeacon.sensor.Row);
//    var dColumn = Math.Abs(sensorBeacon.beacon.Column - sensorBeacon.sensor.Column);

//    var manDistance = dRow + dColumn;

//    var dRowTemp = Math.Abs(rowToCheck - sensorBeacon.sensor.Row);

//    if (dRowTemp > manDistance)
//    {
//        continue;
//    }

//    var remainingDist = manDistance - dRowTemp;

//    for (var i = sensorBeacon.sensor.Column - remainingDist; i <= sensorBeacon.sensor.Column + remainingDist; i++)
//    {
//        var coords = new Coordinates(rowToCheck, i);
//        var exists = space.TryGetValue(coords, out _);
//        if (!exists)
//        {
//            space[coords] = '#';
//        }
//    }
//}

//var beaconless = space.Keys.Count(k => k.Row == rowToCheck && space[k] != 'B' && space[k] != 'S');

//Console.WriteLine(beaconless);
//Console.ReadLine();