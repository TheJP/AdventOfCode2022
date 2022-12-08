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

Console.WriteLine("Task 1: {0}", count);

// Task 2
var previous = new int[4][][][]; // 0=left, 1=top, 2=right, 3=bottom

for (int i = 0; i < 4; ++i)
{
    previous[i] = new int[10][][];
    for (int j = 0; j <= 9; ++j)
    {
        previous[i][j] = new int[grid.Count][];
        for (int y = 0; y < grid.Count; ++y)
        {
            previous[i][j][y] = new int[grid[0].Length];
        }
    }
}

for (int j = 0; j <= 9; ++j)
{
    for (int y = 0; y < grid.Count; ++y)
    {
        previous[0][j][y][0] = 0;
        previous[2][j][y][^1] = grid[y].Length - 1;
    }
    for (int x = 0; x < grid[0].Length; ++x)
    {
        previous[1][j][0][x] = 0;
        previous[3][j][^1][x] = grid.Count - 1;
    }
}

var score = new int[grid.Count][];
for (int y = 0; y < grid.Count; ++y)
{
    score[y] = new int[grid[y].Length];
}

for (int y = 1; y < grid.Count - 1; ++y)
{
    for (int x = 1; x < grid[y].Length - 1; ++x)
    {
        var height = (int)(grid[y][x] - '0');
        var left = x - previous[0][height][y][x - 1];
        var top = y - previous[1][height][y - 1][x];
        score[y][x] = left * top;

        for (int j = 0; j <= height; ++j)
        {
            previous[0][j][y][x] = x;
            previous[1][j][y][x] = y;
        }

        for (int j = height + 1; j <= 9; ++j)
        {
            previous[0][j][y][x] = previous[0][j][y][x - 1];
            previous[1][j][y][x] = previous[1][j][y - 1][x];
        }
    }
}

int maxScore = 0;

for (int y = grid.Count - 2; y >= 1; --y)
{
    for (int x = grid[y].Length - 2; x >= 1; --x)
    {
        var height = (int)(grid[y][x] - '0');
        var right = previous[2][height][y][x + 1] - x;
        var bottom = previous[3][height][y + 1][x] - y;
        score[y][x] *= right * bottom;
        maxScore = Math.Max(maxScore, score[y][x]);

        for (int j = 0; j <= height; ++j)
        {
            previous[2][j][y][x] = x;
            previous[3][j][y][x] = y;
        }

        for (int j = height + 1; j <= 9; ++j)
        {
            previous[2][j][y][x] = previous[2][j][y][x + 1];
            previous[3][j][y][x] = previous[3][j][y + 1][x];
        }
    }
}

// for (int y = 0; y < score.Length; ++y)
// {
//     for (int x = 0; x < score[y].Length; ++x)
//     {
//         Console.Write("{0} ", score[y][x]);
//     }
//     Console.WriteLine();
// }

Console.WriteLine("Task 2: {0}", maxScore);
