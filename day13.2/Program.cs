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
            if (valueStart < i)
            {
                packets.Add(Parse(line[valueStart..i]));
            }
            valueStart = i + 1;
        }
    }

    return new PacketList(packets.ToArray());
}

var packets = new List<Packet>();
for (int i = 0; i < input.Length; i += 3)
{
    var a = Parse(input[i]);
    var b = Parse(input[i + 1]);
    packets.Add(a);
    packets.Add(b);
}

var marker1 = Parse("[[2]]");
var marker2 = Parse("[[6]]");
packets.Add(marker1);
packets.Add(marker2);

int Convert(Ordering o) => o switch
{
    Ordering.Less => -1,
    Ordering.Equal => 0,
    Ordering.Greater => 1,
    _ => throw new InvalidOperationException(),
};

packets.Sort((a, b) => Convert(a.Ordered(b)));

int score = 1;
for (int i = 0; i < packets.Count; ++i)
{
    // Console.WriteLine("{0}", packets[i]);
    if (packets[i] == marker1 || packets[i] == marker2) score *= i + 1;
}

Console.WriteLine("{0}", score);
