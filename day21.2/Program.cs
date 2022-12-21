var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

var tree = new Dictionary<string, Node>();

foreach (var line in input)
{
    var monkey = line[..4];
    var rest = line[6..];
    if (long.TryParse(rest, out var number))
    {
        // Console.WriteLine($"{monkey} <- {number}");
        tree.Add(monkey, new ValueNode() { Value = number });
    }
    else
    {
        var a = rest[..4];
        var op = rest[5];
        var b = rest[7..];
        // Console.WriteLine($"{monkey} <- {a}{op}{b}");
        tree.Add(monkey, new Operation() { A = a, Operator = op, B = b });
    }
}

// Console.WriteLine($"{tree["root"].ComputeValue(tree)}");

bool Contains(string parent, string search)
{
    if (parent == search) return true;
    else if (tree[parent] is Operation op) return Contains(op.A, search) || Contains(op.B, search);
    else return false;
}

var root = (tree["root"] as Operation)!;
root.Operator = '=';

string start;
long targetValue;
if (Contains(root.A, "humn"))
{
    start = root.A;
    targetValue = tree[root.B].ComputeValue(tree);
}
else
{
    start = root.B;
    targetValue = tree[root.A].ComputeValue(tree);
}

long ComputeHuman(string node, long value)
{
    if (node == "humn") return value;
    switch (tree[node])
    {
        case Operation op:
            if (Contains(op.A, "humn")) return op.Operator switch
            {
                '+' => ComputeHuman(op.A, value - tree[op.B].ComputeValue(tree)),
                '-' => ComputeHuman(op.A, value + tree[op.B].ComputeValue(tree)),
                '*' => ComputeHuman(op.A, value / tree[op.B].ComputeValue(tree)),
                '/' => ComputeHuman(op.A, value * tree[op.B].ComputeValue(tree)),
                _ => throw new InvalidOperationException(),
            };
            else return op.Operator switch
            {
                '+' => ComputeHuman(op.B, value - tree[op.A].ComputeValue(tree)),
                '-' => ComputeHuman(op.B, tree[op.A].ComputeValue(tree) - value),
                '*' => ComputeHuman(op.B, value / tree[op.A].ComputeValue(tree)),
                '/' => ComputeHuman(op.B, tree[op.A].ComputeValue(tree) / value),
                _ => throw new InvalidOperationException(),
            };
        default:
            throw new InvalidOperationException();
    };

    throw new InvalidOperationException();
}

Console.WriteLine($"{ComputeHuman(start, targetValue)}");
