using System.Text.RegularExpressions;

var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

// Sensor at x=2, y=18: closest beacon is at x=-2, y=15
var regex = new Regex("Sensor at x=(-?[0-9]+), y=(-?[0-9]+): closest beacon is at x=(-?[0-9]+), y=(-?[0-9]+)");

// const int MaxSearch = 20;
const int MaxSearch = 4000000;
var ranges = new List<(int x1, int x2)>[MaxSearch + 1];
for (int i = 0; i <= MaxSearch; ++i)
{
    ranges[i] = new();
}

var beaconsX = new List<int>();
foreach (var line in input)
{
    var match = regex.Match(line);
    if (!match.Success) throw new InvalidOperationException();

    var sensor = (x: int.Parse(match.Groups[1].Value), y: int.Parse(match.Groups[2].Value));
    var beacon = (x: int.Parse(match.Groups[3].Value), y: int.Parse(match.Groups[4].Value));

    if (beacon.x >= 0 && beacon.x <= MaxSearch &&
        beacon.y >= 0 && beacon.y <= MaxSearch) { ranges[beacon.y].Add((beacon.x, beacon.x)); }

    // Console.WriteLine("{0}/{1} {2}/{3}", sensor.x, sensor.y, beacon.x, beacon.y);

    var distance = Math.Abs(sensor.x - beacon.x) + Math.Abs(sensor.y - beacon.y);

    for (int targetRow = 0; targetRow <= MaxSearch; ++targetRow)
    {
        // Console.WriteLine("{0}", distance);
        var d2 = distance - Math.Abs(sensor.y - targetRow);
        // Console.WriteLine("{0}", d2);
        // Console.WriteLine();

        if (d2 < 0) continue;
        ranges[targetRow].Add((Math.Max(0, sensor.x - d2), Math.Min(MaxSearch, sensor.x + d2)));
    }
}

for (var y = 0; y <= MaxSearch; ++y)
{
    ranges[y].Sort((a, b) => a.x1.CompareTo(b.x1));
    ranges[y].Add((MaxSearch + 1, MaxSearch + 1));
    int currentX = -1;
    foreach (var range in ranges[y])
    {
        // Console.WriteLine("{0} - {1}", range.x1, range.x2);
        if (currentX + 1 < range.x1) Console.WriteLine("{0}", 4_000_000L * (long)(currentX + 1) + (long)y);
        currentX = Math.Max(currentX, range.x2);
    }
    // Console.WriteLine();
}
