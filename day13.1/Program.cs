var input = File.ReadAllLines(Environment.GetCommandLineArgs()[1]);

Packet Parse(string line)
{
    // Console.WriteLine(line);

    if (int.TryParse(line, out var result))
    {
        return new PacketValue(result);
    }

    var packets = new List<Packet>();
    int valueStart = 1;
    int level = 0;
    for (int i = 1; i < line.Length; ++i)
    {
        if (line[i] == '[') ++level;
        if (line[i] == ']') --level;
        if ((line[i] == ',' && level == 0) || level < 0)
        {
            packets.Add(Parse(line[valueStart..i]));
            valueStart = i + 1;
        }
    }

    return new PacketList(packets.ToArray());
}

int score = 0;
for (int i = 0; i < input.Length; i += 3)
{
    var a = Parse(input[i]);
    var b = Parse(input[i + 1]);
    if (a.Ordered(b) == Ordering.Less)
    {
        score += (i / 3) + 1;
    }
}

Console.WriteLine("{0}", score);
