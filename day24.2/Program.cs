var input = File.ReadAllLines(Environment.GetCommandLineArgs()[1]);
// Add border
input = new string[] { new('#', input[0].Length) }
    .Concat(input)
    .Concat(new string[] { new('#', input[0].Length) })
    .ToArray();

var blizzards = new List<(int x, int y)>();
var directions = new List<(int x, int y)>();
for (int y = 0; y < input.Length; ++y)
{
    for (int x = 0; x < input[0].Length; ++x)
    {
        switch (input[y][x])
        {
            case '>':
                blizzards.Add((x, y));
                directions.Add((1, 0));
                break;
            case '<':
                blizzards.Add((x, y));
                directions.Add((-1, 0));
                break;
            case 'v':
                blizzards.Add((x, y));
                directions.Add((0, 1));
                break;
            case '^':
                blizzards.Add((x, y));
                directions.Add((0, -1));
                break;
            default:
                break;
        }
    }
}

int minute = 0;
var now = new HashSet<(int x, int y)>();
var next = new HashSet<(int x, int y)>();
next.Add((1, 1));

bool[,] obstacle = new bool[input.Length, input[0].Length];
(int x, int y)[] moves = { (0, 0), (1, 0), (-1, 0), (0, 1), (0, -1) };

var goals = new Queue<(int x, int y)>();
goals.Enqueue((input[0].Length - 2, input.Length - 2));
goals.Enqueue((1, 1));
goals.Enqueue((input[0].Length - 2, input.Length - 2));

while (next.Count > 0)
{
    ++minute;
    obstacle = new bool[input.Length, input[0].Length];
    for (int b = 0; b < blizzards.Count; ++b)
    {
        var x = blizzards[b].x + directions[b].x;
        var y = blizzards[b].y + directions[b].y;
        if (y == 1) y = input.Length - 3;
        if (y == input.Length - 2) y = 2;
        if (x == 0) x = input[0].Length - 2;
        if (x == input[0].Length - 1) x = 1;
        blizzards[b] = (x, y);
        obstacle[y, x] = true;
    }

    // for (int y = 0; y < input.Length; ++y)
    // {
    //     for (int x = 0; x < input[y].Length; ++x)
    //     {
    //         if (obstacle[y, x]) Console.Write('#');
    //         else if (input[y][x] == '#') Console.Write('#');
    //         else Console.Write('.');
    //     }
    //     Console.WriteLine();
    // }
    // Console.WriteLine();

    now = next;
    next = new();
    foreach (var current in now)
    {
        if (current == goals.Peek())
        {
            goals.Dequeue();
            if (goals.Count == 0)
            {
                Console.WriteLine(minute);
                return;
            }
            else
            {
                next.Clear();
                foreach (var move in moves)
                {
                    next.Add((current.x + move.x, current.y + move.y));
                }
                break;
            }
        }

        if (obstacle[current.y, current.x]) continue;
        if (input[current.y][current.x] == '#') continue;

        foreach (var move in moves)
        {
            next.Add((current.x + move.x, current.y + move.y));
        }
    }
}

// Console.WriteLine();