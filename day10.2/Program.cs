var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

int x = 1;
int cycles = 0;

var xs = new List<int>();
xs.Add(x);

foreach (var line in input)
{
    if (line == "noop")
    {
        ++cycles;
        xs.Add(x);
        continue;
    }
    if (!line.StartsWith("addx")) throw new InvalidOperationException();

    xs.Add(x);

    var add = int.Parse(line.Split()[^1]);
    x += add;
    cycles += 2;

    xs.Add(x);
}

int column = 0;
for (int i = 0; i < cycles; ++i)
{
    x = xs[i];
    if (x - 1 <= column && column <= x + 1)
    {
        Console.Write("#");
    }
    else
    {
        Console.Write(".");
    }

    column = (column + 1) % 40;
    if (column == 0) {
        Console.WriteLine();
    }
}
