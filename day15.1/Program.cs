using System.Text.RegularExpressions;

// const int TargetRow = 10;
const int TargetRow = 2_000_000;
var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

// Sensor at x=2, y=18: closest beacon is at x=-2, y=15
var regex = new Regex("Sensor at x=(-?[0-9]+), y=(-?[0-9]+): closest beacon is at x=(-?[0-9]+), y=(-?[0-9]+)");

const int Negative = -20_000_000;
var reached = new bool[40_000_000];

var score = 0;
var beaconsX = new List<int>();
foreach (var line in input)
{
    var match = regex.Match(line);
    if (!match.Success) throw new InvalidOperationException();

    var sensor = (x: int.Parse(match.Groups[1].Value), y: int.Parse(match.Groups[2].Value));
    var beacon = (x: int.Parse(match.Groups[3].Value), y: int.Parse(match.Groups[4].Value));

    if (beacon.y == TargetRow) beaconsX.Add(beacon.x);

    // Console.WriteLine("{0}/{1} {2}/{3}", sensor.x, sensor.y, beacon.x, beacon.y);

    var distance = Math.Abs(sensor.x - beacon.x) + Math.Abs(sensor.y - beacon.y);

    // Console.WriteLine("{0}", distance);
    distance -= Math.Abs(sensor.y - TargetRow);
    // Console.WriteLine("{0}", distance);
    // Console.WriteLine();

    if (distance < 0) continue;

    for (int x = sensor.x - distance; x <= sensor.x + distance; ++x) {
        int rx = x - Negative;
        if (rx < 0 || rx > reached.Length) Console.WriteLine("{0}", rx);
        if (reached[rx]) continue;

        reached[rx] = true;
        ++score;
    }
}

foreach (var x in beaconsX.Distinct()) {
    if (reached[x - Negative]) --score;
}

// for (int x = -4; x < 27; ++x) {
//     Console.Write("{0}", reached[x - Negative] ? '#' : '.');
// }
// Console.WriteLine();

Console.WriteLine("{0}", score);