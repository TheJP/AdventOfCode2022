var input = File.ReadAllLines(Environment.GetCommandLineArgs()[1])[0];

const int width = 7;

var shapes = new string[][]
{
    new[]{ "####" },
    new[]
    {
        ".#.",
        "###",
        ".#.",
    },
    new[]
    {
        "..#",
        "..#",
        "###",
    },
    new[]{ "#","#","#","#" },
    new[]
    {
        "##",
        "##",
    }
}.Select(shape => shape.Select(row => row.Select(c => c == '#').ToArray()).ToArray()).ToArray();

const int loop = 2022;
const int bufferHeight = 4 + 4;
int highest = 0;
int inputPosition = 0;

int currentShape = -1;
int x = 0;
int y = 0;

List<bool[]> tetris = new();
for (int i = 0; i < bufferHeight; ++i) tetris.Add(new bool[width]);

void SpawnNextShape()
{
    currentShape = (currentShape + 1) % shapes.Length;
    x = 2;
    y = highest + 3;
}

bool Collision()
{
    if (x < 0 || x + shapes[currentShape][0].Length > width || y < 0) return true;

    for (int py = y; py < y + shapes[currentShape].Length; ++py)
    {
        for (int px = x; px < x + shapes[currentShape][0].Length; ++px)
        {
            // Console.WriteLine($"{currentShape} {shapes[currentShape].Length - (py - y) - 1} {px - x} && tetris[{py}][{px}]");
            if (shapes[currentShape][shapes[currentShape].Length - (py - y) - 1][px - x] &&
                tetris[py][px])
                return true;
        }
    }

    return false;
}

void SimulateShape()
{
    while (!Collision())
    {
        x += input[inputPosition] == '<' ? -1 : 1;
        if (Collision()) x -= input[inputPosition] == '<' ? -1 : 1;
        inputPosition = (inputPosition + 1) % input.Length;

        --y;
    }
    ++y;
    highest = Math.Max(highest, y + shapes[currentShape].Length);
    while (tetris.Count < highest + bufferHeight) tetris.Add(new bool[width]);

    for (int py = y; py < y + shapes[currentShape].Length; ++py)
    {
        for (int px = x; px < x + shapes[currentShape][0].Length; ++px)
        {
            if (shapes[currentShape][shapes[currentShape].Length - (py - y) - 1][px - x])
            {
                tetris[py][px] = true;
            }
        }
    }
}

#pragma warning disable CS8321
void PrintTetris(int max = 20)
{
    for (int ty = highest; ty >= 0; --ty)
    {
        for (int tx = 0; tx < width; ++tx)
        {
            Console.Write(tetris[ty][tx] ? '#' : '.');
        }
        Console.WriteLine();
        if (highest - ty + 1 >= max) break;
    }
    Console.WriteLine();
}
#pragma warning restore CS8321

for (int i = 0; i < loop; ++i)
{
    SpawnNextShape();
    SimulateShape();
    // PrintTetris();
}

Console.WriteLine(highest);
