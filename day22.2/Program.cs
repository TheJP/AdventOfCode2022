var input = File.ReadAllLines(Environment.GetCommandLineArgs()[1]);

string path = input[^1];
string[] grid = input[0..^2];

var instructions = new List<(int length, char rotation)>();

int numberStart = 0;
for (int i = 0; i < path.Length; ++i)
{
    if (char.IsDigit(path[i])) continue;
    instructions.Add((int.Parse(path[numberStart..i]), path[i]));
    numberStart = i + 1;
}
instructions.Add((int.Parse(path[numberStart..]), '_'));

// Console.WriteLine($"{instructions[^2]}");
// Console.WriteLine($"{instructions[^1]}");

const int Right = 0;
const int Down = 1;
const int Left = 2;
const int Up = 3;
int facing = 0;

int y = 0;
int x = grid[0].Select((tile, i) => (tile, i)).First(t => t.tile == '.').i;

var wrappingH = new Dictionary<(int x, int y), ((int x, int y) next, int facing)>();
var wrappingV = new Dictionary<(int x, int y), ((int x, int y) next, int facing)>();

for (int i = 0; i < 50; ++i)
{
    // (1, 0) Left Edge <-> (0, 2) Rev. Left Edge
    {
        var next = (x: 0, y: (49-i) + 100);
        wrappingH.Add((50, i), (next, Right));
        wrappingH.Add(next, ((50, i), Right));
    }

    // (1, 1) Left Edge <-> (0, 2) Top Edge
    {
        var left = (x: 50, y: i + 50);
        var right = (x: i, y: 100);
        wrappingH.Add(left, (right, Down));
        wrappingV.Add(right, (left, Right));
    }

    // (1, 0) Top Edge <-> (0, 3) Left Edge
    {
        var left = (x: i + 50, y: 0);
        var right = (x: 0, y: i + 150);
        wrappingV.Add(left, (right, Right));
        wrappingH.Add(right, (left, Down));
    }

    // (1, 2) Bottom Edge <-> (0, 3) Right Edge
    {
        var left = (x: i + 50, y: 149);
        var right = (x: 49, y: i + 150);
        wrappingV.Add(left, (right, Left));
        wrappingH.Add(right, (left, Up));
    }

    // (2, 0) Bottom Edge <-> (1, 1) Right Edge
    {
        var left = (x: i + 100, y: 49);
        var right = (x: 99, y: i + 50);
        wrappingV.Add(left, (right, Left));
        wrappingH.Add(right, (left, Up));
    }

    // (2, 0) Rev. Right Edge <-> (1, 2) Right Edge
    {
        var left = (x: 149, y: 49 - i);
        var right = (x: 99, y: i + 100);
        wrappingH.Add(left, (right, Left));
        wrappingH.Add(right, (left, Left));
    }

    // (2, 0) Top Edge <-> (0, 3) Bottom Edge
    {
        var left = (x: i + 100, y: 0);
        var right = (x: i, y: 199);
        wrappingV.Add(left, (right, Up));
        wrappingV.Add(right, (left, Down));
    }
}

// Console.WriteLine($"{x}/{y}");
// Console.WriteLine($"{firstY(3)} {lastY(3)} {lastY(15)}");

foreach (var instruction in instructions)
{
    for (int i = 0; i < instruction.length; ++i)
    {
        ((int x, int y) coords, int facing) next;
        switch (facing)
        {
            case Right:
                if (x + 1 < grid[y].Length) next = ((x + 1, y), facing);
                else next = wrappingH[(x, y)];
                break;
            case Left:
                if (x > 0 && grid[y][x - 1] != ' ') next = ((x - 1, y), facing);
                else next = wrappingH[(x, y)];
                break;
            case Down:
                if (y + 1 < grid.Length && x < grid[y + 1].Length && grid[y + 1][x] != ' ') next = ((x, y + 1), facing);
                else next = wrappingV[(x, y)];
                break;
            case Up:
                if (y > 0 && x < grid[y - 1].Length && grid[y - 1][x] != ' ') next = ((x, y - 1), facing);
                else next = wrappingV[(x, y)];
                break;
            default:
                throw new InvalidOperationException();
        }

        if (grid[next.coords.y][next.coords.x] == '#') break;
        (x, y) = next.coords;
        facing = next.facing;
    }

    switch (instruction.rotation)
    {
        case 'L':
            facing -= 1;
            if (facing < 0) facing = 3;
            break;
        case 'R':
            facing = (facing + 1) % 4;
            break;
        case '_':
            break;
        default:
            throw new InvalidOperationException();
    }
}

Console.WriteLine($"{1000 * (y + 1) + 4 * (x + 1) + facing}");
