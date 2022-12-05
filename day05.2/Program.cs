using var input = new StreamReader(Environment.GetCommandLineArgs()[1]);
string? line;
List<char>[]? start = null;

while ((line = input.ReadLine()) != null && !string.IsNullOrWhiteSpace(line))
{
    start ??= new List<char>[line.Length];
    for (int i = 0; i < line.Length; ++i)
    {
        start[i] ??= new List<char>();
        if (char.IsLetter(line[i]))
        {
            start[i].Add(line[i]);
        }
    }
}

var stacks = new Stack<char>[start?.Length ?? 0];
for (int i = 0; i < stacks.Length; ++i)
{
    start![i].Reverse();
    stacks[i] = new Stack<char>(start![i]);
}

while ((line = input.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) return;
    var command = line.Split().Select(int.Parse).ToArray();
    if (command.Length != 3) throw new InvalidOperationException();

    var crates = new List<char>();
    for (int i = 0; i < command[0]; ++i) {
        crates.Add(stacks[command[1] - 1].Pop());
    }

    crates.Reverse();
    foreach (var crate in crates) {
        stacks[command[2] - 1].Push(crate);
    }
}

Console.WriteLine("{0}", string.Join(null, stacks.Select(s => s.Peek())));
