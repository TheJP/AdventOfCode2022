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

int repeat = shapes.Length * input.Length;
int baseRepeat = repeat * 100;

for (int i = 0; i < baseRepeat; ++i)
{
    SpawnNextShape();
    SimulateShape();
}

int baseHeight = highest;

var addedJ = new List<int>();
var period = -1;
while (true)
{
    if (addedJ.Count % 100 == 0) Console.WriteLine(addedJ.Count);

    var oldHighest = highest;
    for (int i = 0; i < repeat; ++i)
    {
        SpawnNextShape();
        SimulateShape();
    }
    addedJ.Add(highest - oldHighest);

    if (addedJ.Count % 2 == 0 && addedJ.Count >= 4)
    {
        var count = addedJ.Count / 2;
        var equal = true;
        for (int i = 0; i < count; ++i)
        {
            if (addedJ[i] != addedJ[i + count])
            {
                equal = false;
                break;
            }
        }
        if (equal) {
            period = count;
            break;
        }
    }
}

Console.WriteLine($"Period: {period}");

// verify
// for (int j = 0; j < period; ++j) {
//     var oldHighest = highest;
//     for (int i = 0; i < repeat; ++i)
//     {
//         SpawnNextShape();
//         SimulateShape();
//     }
//     if (addedJ[j] != highest - oldHighest) {
//         Console.WriteLine("nope!");
//         break;
//     }
// }

long heightPerPeriod = (highest - baseHeight) / 2;

long numberOfIterations = baseRepeat + period * 2 * repeat;
const long targetIterations = 1_000_000_000_000L;

long iterationsToDo = targetIterations - numberOfIterations;
long periodsToDo = iterationsToDo / (period * repeat);
long intermediateHeight = baseHeight + (periodsToDo + 2) * heightPerPeriod;

long remainingIterations = iterationsToDo - (periodsToDo * period * repeat);
Console.WriteLine($"remainingIterations: {remainingIterations}");

int beforeRemaining = highest;
for (int i = 0; i < remainingIterations; ++i) {
    SpawnNextShape();
    SimulateShape();
}
long remainingHeight = highest - beforeRemaining;
long finalHeight = intermediateHeight + remainingHeight;

Console.WriteLine($"Result (Final Height): {finalHeight}");
