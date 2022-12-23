const int Rounds = 1000;

var input = File.ReadAllLines(Environment.GetCommandLineArgs()[1]);

int height = input.Length + 2 * Rounds;
int width = input[0].Length + 2 * Rounds;
bool[][] grid = new bool[height][];

#pragma warning disable CS8321
void Print()
{
    for (int y = 0; y < height; ++y)
    {
        for (int x = 0; x < width; ++x) Console.Write(grid[y][x] ? '#' : '.');
        Console.WriteLine();
    }
    Console.WriteLine();
}
#pragma warning restore CS8321

for (int i = 0; i < input.Length; ++i)
{
    grid[i + Rounds] = new bool[Rounds]
        .Concat(input[i].Select(c => c == '#'))
        .Concat(new bool[Rounds])
        .ToArray();
}

for (int i = 0; i < Rounds; ++i)
{
    grid[i] = new bool[width];
    grid[height - i - 1] = new bool[width];
}

(int x, int y)[] adjacent =
{
    (-1, -1), (0, -1), (1, -1),
    (-1, 0), (1, 0),
    (-1, 1), (0, 1), (1, 1),
};
((int x, int y)[] check, (int x, int y) move)[] moves = {
    (new[] { (-1, -1), (0, -1), (1, -1) }, (0, -1)), // N,NE,NW -> N
    (new[] { (-1, 1), (0, 1), (1, 1) }, (0, 1)), // S,SE,SW -> S
    (new[] { (-1, -1), (-1, 0), (-1, 1) }, (-1, 0)), // W,NW,SW -> W
    (new[] { (1, -1), (1, 0), (1, 1) }, (1, 0)), // E,NE,SE -> E
};

var finished = false;
for (int round = 0; round < Rounds; ++round)
{
    if (finished)
    {
        Console.WriteLine($"Task 2: {round}");
        break;
    }
    finished = true;
    // Print(); Console.WriteLine();

    var next = new int[height][];
    for (int i = 0; i < height; ++i) next[i] = new int[width];
    var grid2 = new bool[height][];
    for (int i = 0; i < height; ++i) grid2[i] = new bool[width];

    for (int y = 0; y < height; ++y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (!grid[y][x]) continue;
            if (!adjacent.Any(adj => grid[y + adj.y][x + adj.x])) continue;
            finished = false;

            foreach (var move in moves)
            {
                if (!move.check.Any(check => grid[y + check.y][x + check.x]))
                {
                    // Console.WriteLine($"{x}/{y} -> {x + move.move.x}/{y + move.move.y}");
                    ++next[y + move.move.y][x + move.move.x];
                    break;
                }
            }
        }
    }

    for (int y = 0; y < height; ++y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (!grid[y][x]) continue;
            if (!adjacent.Any(adj => grid[y + adj.y][x + adj.x]))
            {
                grid2[y][x] = true;
                continue;
            }

            var moved = false;
            foreach (var move in moves)
            {
                moved = !move.check.Any(check => grid[y + check.y][x + check.x]);
                if (!moved) continue;

                // Console.WriteLine($"{x}/{y} -> {x + move.move.x}/{y + move.move.y} [{next[y + move.move.y][x + move.move.x]}]");
                if (next[y + move.move.y][x + move.move.x] != 1) grid2[y][x] = true;
                else grid2[y + move.move.y][x + move.move.x] = true;

                break;
            }

            if (!moved) grid2[y][x] = true;
        }
    }

    grid = grid2;
    moves = moves.Skip(1).Append(moves[0]).ToArray();

    if (round == 9)
    {
        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                if (!grid[y][x]) continue;
                (minX, minY) = (Math.Min(minX, x), Math.Min(minY, y));
                (maxX, maxY) = (Math.Max(maxX, x), Math.Max(maxY, y));
            }
        }

        int countGround = 0;
        for (int y = minY; y <= maxY; ++y)
            for (int x = minX; x <= maxX; ++x)
                if (!grid[y][x]) ++countGround;


        Console.WriteLine($"Task 1: {countGround}");
    }
}
// Print(); Console.WriteLine();
