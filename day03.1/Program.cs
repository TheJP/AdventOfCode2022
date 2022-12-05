using var input = new StreamReader(Environment.GetCommandLineArgs()[1]);
long score = 0;
string? line;
while ((line = input.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) return;
    if (line.Length % 2 != 0) throw new InvalidOperationException();
    var first = line.Substring(0, line.Length / 2);
    var second = line.Substring(line.Length / 2);

    var a = first.ToArray();
    var b = second.ToArray();
    Array.Sort(a);
    Array.Sort(b);

    var (i, j) = (0, 0);
    while (a[i] != b[j]) {
        if (a[i] < b[j]) ++i;
        else ++j;
    }
    var priority = a[i] switch {
        >= 'a' and <= 'z' => a[i] - 'a' + 1,
        >= 'A' and <= 'Z' => a[i] - 'A' + 27,
        _ => throw new InvalidOperationException(),
    };
    score += priority;
}

Console.WriteLine("{0}", score);
