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

const int Rigth = 0;
const int Down = 1;
const int Left = 2;
const int Up = 3;
int facing = 0;

int y = 0;
int x = grid[0].Select((tile, i) => (tile, i)).First(t => t.tile == '.').i;

int firstX(int y) => grid[y].Select((tile, i) => (tile, i)).First(t => t.tile != ' ').i;
int firstY(int x) => Enumerable.Range(0, grid.Length)
    .First(i => x < grid[i].Length && grid[i][x] != ' ');
int lastY(int x) => grid.Length - 1 - Enumerable.Range(0, grid.Length)
    .First(i => x < grid[grid.Length - i - 1].Length && grid[grid.Length - i - 1][x] != ' ');

// Console.WriteLine($"{x}/{y}");
// Console.WriteLine($"{firstY(3)} {lastY(3)} {lastY(15)}");

foreach (var instruction in instructions)
{
    for (int i = 0; i < instruction.length; ++i)
    {
        (int x, int y) next;
        switch (facing)
        {
            case Rigth:
                if (x + 1 < grid[y].Length) next = (x + 1, y);
                else next = (firstX(y), y);
                break;
            case Left:
                if (x > 0 && grid[y][x - 1] != ' ') next = (x - 1, y);
                else next = (grid[y].Length - 1, y);
                break;
            case Down:
                if (y + 1 < grid.Length && x < grid[y + 1].Length && grid[y + 1][x] != ' ') next = (x, y + 1);
                else next = (x, firstY(x));
                break;
            case Up:
                if (y > 0 && x < grid[y - 1].Length && grid[y - 1][x] != ' ') next = (x, y - 1);
                else next = (x, lastY(x));
                break;
            default:
                throw new InvalidOperationException();
        }

        if (grid[next.y][next.x] == '#') break;
        (x, y) = next;
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
