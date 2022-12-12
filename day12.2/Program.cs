var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

var grid = new List<string>();
(int x, int y)? s = null, e = null;

int row = 0;
foreach (var l in input)
{
    var line = l;
    ++row;
    var indexE = line.IndexOf('E');
    if (indexE >= 0)
    {
        line = line.Replace('E', 'z');
        e = (indexE + 1, row);
    }
    var indexS = line.IndexOf('S');
    if (indexS >= 0)
    {
        line = line.Replace('S', 'a');
        s = (indexS + 1, row);
    }
    grid.Add($"{(char)('z' + 10)}{line}{(char)('z' + 10)}");
}
grid.Insert(0, new string(grid[0].Select(_ => (char)('z' + 10)).ToArray()));
grid.Add(grid[0]);

// Console.WriteLine("{0} {1}", s, e);
(int x, int y) start = s ?? throw new InvalidOperationException();
(int x, int y) end = e ?? throw new InvalidOperationException();

int Calculate((int x, int y) start)
{
    var distances = grid.Select(row => row.Select(_ => (int)short.MaxValue).ToArray()).ToArray();

    var pq = new PriorityQueue<(int x, int y), int>();
    pq.Enqueue(start, 0);
    distances[start.y][start.x] = 0;

    while (pq.Count > 0)
    {
        if (!pq.TryDequeue(out var current, out var distance))
        {
            throw new InvalidOperationException();
        }

        if (current.x == end.x && current.y == end.y)
        {
            break;
        }

        (int x, int y)[] permutations = { (1, 0), (-1, 0), (0, 1), (0, -1) };
        foreach (var p in permutations)
        {
            var next = (x: current.x + p.x, y: current.y + p.y);
            if (grid[current.y][current.x] + 1 < grid[next.y][next.x]) continue;
            var d = distances[current.y][current.x] + 1;
            if (d < distances[next.y][next.x])
            {
                distances[next.y][next.x] = d;
                pq.Enqueue(next, d);
            }
        }
    }

    return distances[end.y][end.x];
}

// Console.WriteLine("{0}", Calculate(start));

var routes = new List<int>();
for (int y = 1; y < grid.Count - 1; ++y)
{
    for (int x = 1; x < grid[y].Length - 1; ++x)
    {
        if (grid[y][x] != 'a') continue;
        routes.Add(Calculate((x, y)));
    }
}

Console.WriteLine("{0}", routes.Min());
