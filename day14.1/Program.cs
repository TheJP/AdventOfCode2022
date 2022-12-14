var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

char[,] grid = new char[1000, 1000];

for (int y = 0; y < 1000; ++y)
{
    for (int x = 0; x < 1000; ++x)
    {
        grid[y, x] = '.';
    }
}

foreach (var line in input)
{
    var parts = line
        .Split("->")
        .Select(p => p.Trim().Split(','))
        .Select(p => (x: int.Parse(p[0]), y: int.Parse(p[1])))
        .ToArray();

    for (int i = 1; i < parts.Length; ++i)
    {
        var yFrom = Math.Min(parts[i - 1].y, parts[i].y);
        var yTo = Math.Max(parts[i - 1].y, parts[i].y);
        var xFrom = Math.Min(parts[i - 1].x, parts[i].x);
        var xTo = Math.Max(parts[i - 1].x, parts[i].x);
        for (int y = yFrom; y <= yTo; ++y)
        {
            for (int x = xFrom; x <= xTo; ++x)
            {
                grid[y, x] = '#';
            }
        }
    }
}

var start = (x: 500, y: 0);
var count = 0;

while (true)
{
    var x = start.x;
    var y = start.y;

    while (y + 1 < 1000)
    {
        if (grid[y + 1, x] == '.') ++y;
        else if (x > 0 && grid[y + 1, x - 1] == '.')
        {
            ++y; --x;
        }
        else if (x + 1 < 1000 && grid[y + 1, x + 1] == '.')
        {
            ++y; ++x;
        }
        else
        {
            break;
        }
    }

    if (y + 1 >= 1000) break;

    grid[y, x] = '0';
    ++count;
}

Console.WriteLine("{0}", count);
