using Shared;
using System.Text;
using System.Text.RegularExpressions;

using var reader = new StreamReader("input.txt", Encoding.UTF8);

var regex = new Regex(@"x=([-]*\d+).+y=([-]*\d+).+x=([-]*\d+).+y=([-]*\d+)");
var inputString = reader.ReadToEnd();

var lines = inputString.Split(Environment.NewLine);

var sensorsBeacons = lines.Select(line =>
{
    var match = regex.Match(line);

    var sensor = new Coordinates(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[1].Value));
    var beacon = new Coordinates(int.Parse(match.Groups[4].Value), int.Parse(match.Groups[3].Value));

    return (beacon, sensor);
}).ToList();

var space = new Dictionary<Coordinates, char>();
var checkedSpaces = 0;

var rowToCheck = 2000000;

foreach (var sensorBeacon in sensorsBeacons)
{
    space[sensorBeacon.sensor] = 'S';
    space[sensorBeacon.beacon] = 'B';

    var dRow = Math.Abs(sensorBeacon.beacon.Row - sensorBeacon.sensor.Row);
    var dColumn = Math.Abs(sensorBeacon.beacon.Column - sensorBeacon.sensor.Column);

    var manDistance = dRow + dColumn;

    var dRowTemp = Math.Abs(rowToCheck - sensorBeacon.sensor.Row);

    if (dRowTemp > manDistance)
    {
        continue;
    }

    var remainingDist = manDistance - dRowTemp;

    for (var i = sensorBeacon.sensor.Column - remainingDist; i <= sensorBeacon.sensor.Column + remainingDist; i++)
    {
        var coords = new Coordinates(rowToCheck, i);
        var exists = space.TryGetValue(coords, out _);
        if (!exists)
        {
            space[coords] = '#';
        }

        //for (var j = sensorBeacon.sensor.Column - manDistance; j <= sensorBeacon.sensor.Column + manDistance; j++)
        //{
        //    var dRowTemp = Math.Abs(i - sensorBeacon.sensor.Row);
        //    var dColumnTemp = Math.Abs(j - sensorBeacon.sensor.Column);
        //    if (dColumnTemp + dRowTemp <= manDistance)
        //    {
        //        var coords = new Coordinates(i, j);
        //        var exists = space.TryGetValue(coords, out _);
        //        if (!exists)
        //        {
        //            space[coords] = '#';
        //        }
        //    }
        //}
    }
}

var beaconless = space.Keys.Count(k => k.Row == rowToCheck && space[k] != 'B' && space[k] != 'S');

Console.WriteLine(beaconless);
Console.ReadLine();