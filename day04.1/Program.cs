using var input = new StreamReader(Environment.GetCommandLineArgs()[1]);
long score = 0;
string? line;

(int, int) GetRange(string range)
{
    var values = range.Split('-', 2);
    return (int.Parse(values[0]), int.Parse(values[1]));
}

while ((line = input.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) return;
    var ranges = line.Split(',', 2).Select(GetRange).ToArray();
    switch ((ranges[0], ranges[1]))
    {
        case var ((a, b), (x, y)) when
                (a <= x && y <= b) ||
                (x <= a && b <= y):
            ++score;
            break;
        default:
            break;
    }
}

Console.WriteLine("{0}", score);
