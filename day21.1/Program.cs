var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

var tree = new Dictionary<string, Node>();

foreach (var line in input)
{
    var monkey = line[..4];
    var rest = line[6..];
    if (long.TryParse(rest, out var number)) {
        // Console.WriteLine($"{monkey} <- {number}");
        tree.Add(monkey, new ValueNode() { Value = number });
    } else {
        var a = rest[..4];
        var op = rest[5];
        var b = rest[7..];
        // Console.WriteLine($"{monkey} <- {a}{op}{b}");
        tree.Add(monkey, new Operation() { A = a, Operator = op, B = b });
    }
}

Console.WriteLine($"{tree["root"].ComputeValue(tree)}");
