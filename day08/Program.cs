using var input = new StreamReader(Environment.GetCommandLineArgs()[1]);

var grid = new List<string>();

string? line;
while ((line = input.ReadLine()) != null)
{
    grid.Add(line);
}

var count = 0;
var visible = new bool[grid.Count][];
var highest = new char[4][][]; // 0=left, 1=top, 2=right, 3=bottom
for (int i = 0; i < 4; ++i)
{
    highest[i] = new char[grid.Count][];
}

for (int y = 0; y < grid.Count; ++y)
{
    visible[y] = new bool[grid[0].Length];
    visible[y][0] = true;
    visible[y][^1] = true;
    for (int i = 0; i < 4; ++i)
    {
        highest[i][y] = new char[grid[0].Length];
    }
    highest[0][y][0] = grid[y][0];
    highest[2][y][^1] = grid[y][^1];
    count += 2;
}
for (int x = 1; x < grid[0].Length - 1; ++x)
{
    visible[0][x] = true;
    visible[^1][x] = true;
    highest[1][0][x] = grid[0][x];
    highest[3][^1][x] = grid[^1][x];
    count += 2;
}

for (int y = 1; y < grid.Count - 1; ++y)
{
    for (int x = 1; x < grid[y].Length - 1; ++x)
    {
        if (highest[1][y - 1][x] < grid[y][x] || highest[0][y][x - 1] < grid[y][x])
        {
            visible[y][x] = true;
            ++count;
        }
        highest[1][y][x] = (char)Math.Max(grid[y][x], highest[1][y - 1][x]);
        highest[0][y][x] = (char)Math.Max(grid[y][x], highest[0][y][x - 1]);
    }
}

for (int y = grid.Count - 2; y >= 1; --y)
{
    for (int x = grid[y].Length - 2; x >= 1; --x)
    {
        if (highest[3][y + 1][x] < grid[y][x] || highest[2][y][x + 1] < grid[y][x])
        {
            if (!visible[y][x])
            {
                visible[y][x] = true;
                ++count;
            }
        }
        highest[3][y][x] = (char)Math.Max(grid[y][x], highest[3][y + 1][x]);
        highest[2][y][x] = (char)Math.Max(grid[y][x], highest[2][y][x + 1]);
    }
}

// for (int y = 0; y < grid.Count; ++y)
// {
//     for (int x = 0; x < grid[y].Length; ++x)
//     {
//         Console.Write("{0}", visible[y][x] ? 'x' : ' ');
//     }
//     Console.WriteLine();
// }

Console.WriteLine("{0}", count);
