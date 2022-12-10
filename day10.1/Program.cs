var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

int x = 1;
int cycles = 0;

var xs = new List<int>();
xs.Add(x);

int score = 0;

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

score = xs[19] * 20 + xs[59] * 60 + xs[99] * 100 + xs[139] * 140 + xs[179] * 180 + xs[219] * 220;

Console.WriteLine("{0}", score);
