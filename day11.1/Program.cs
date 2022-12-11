using var input = new StreamReader(Environment.GetCommandLineArgs()[1]);
string line;

var monkeys = new List<Monkey>();

while ((line = input.ReadLine()) != null)
{
    if (!line.StartsWith("Monkey")) throw new InvalidOperationException();

    line = input.ReadLine();
    if (!line?.StartsWith("  Starting items: ") ?? true) throw new InvalidOperationException();
    var items = line.Substring("  Starting items: ".Length)
        .Split(',')
        .Select(item => int.Parse(item.Trim()));

    line = input.ReadLine();
    if (!line?.StartsWith("  Operation: new = ") ?? true) throw new InvalidOperationException();
    var operationParts = line.Substring("  Operation: new = ".Length).Split(' ').ToArray();
    var operation = (int old) =>
    {
        var lhs = operationParts[0] == "old" ? old : int.Parse(operationParts[0]);
        var rhs = operationParts[2] == "old" ? old : int.Parse(operationParts[2]);
        return operationParts[1][0] switch
        {
            '+' => lhs + rhs,
            '*' => lhs * rhs,
            _ => throw new InvalidOperationException(),
        };
    };

    line = input.ReadLine();
    if (!line?.StartsWith("  Test: divisible by ") ?? true) throw new InvalidOperationException();
    var test = int.Parse(line.Split(' ')[^1]);

    line = input.ReadLine();
    if (!line?.StartsWith("    If true: throw to monkey ") ?? true) throw new InvalidOperationException();
    var monkeyTrue = int.Parse(line.Split(' ')[^1]);

    line = input.ReadLine();
    if (!line?.StartsWith("    If false: throw to monkey ") ?? true) throw new InvalidOperationException();
    var monkeyFalse = int.Parse(line.Split(' ')[^1]);

    var monkey = new Monkey
    {
        Items = new(items),
        Operation = operation,
        Test = test,
        MonkeyTrue = monkeyTrue,
        MonkeyFalse = monkeyFalse,
    };
    monkeys.Add(monkey);

    input.ReadLine(); // empty line
}

var counter = monkeys.Select(_ => 0).ToArray();

for (int i = 0; i < 20; ++i) {
    int monkeyIndex = 0;
    foreach(var monkey in monkeys) {
        while (monkey.Items.Count > 0) {
            ++counter[monkeyIndex];
            var item = monkey.Items.Dequeue();
            item = monkey.Operation(item);
            item /= 3;
            var test = item % monkey.Test == 0;
            if (test) {
                monkeys[monkey.MonkeyTrue].Items.Enqueue(item);
            } else {
                monkeys[monkey.MonkeyFalse].Items.Enqueue(item);
            }
        }
        ++monkeyIndex;
    }
}

Array.Sort(counter);

Console.WriteLine("{0}", counter[^1] * counter[^2]);
