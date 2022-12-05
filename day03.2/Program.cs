using var input = new StreamReader(Environment.GetCommandLineArgs()[1]);
long score = 0;
string? line;
var lines = new List<string>();
while ((line = input.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) return;
    lines.Add(line);
}

foreach (var group in lines.Chunk(3)) {
    if (group.Length != 3) throw new InvalidOperationException();

    var a = group[0].ToArray();
    var b = group[1].ToArray();
    var c = group[2].ToArray();
    Array.Sort(a);
    Array.Sort(b);
    Array.Sort(c);

    var (u, v, w) = (0, 0, 0);
    while (a[u] != b[v] || b[v] != c[w]) {
        if (a[u] < b[v]) ++u;
        else if (b[v] < c[w]) ++v;
        else ++w;
    }
    var priority = a[u] switch {
        >= 'a' and <= 'z' => a[u] - 'a' + 1,
        >= 'A' and <= 'Z' => a[u] - 'A' + 27,
        _ => throw new InvalidOperationException(),
    };
    score += priority;
}

Console.WriteLine("{0}", score);
