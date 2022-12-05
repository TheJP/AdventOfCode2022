using var input = new StreamReader(Environment.GetCommandLineArgs()[1]);
long score = 0;
string? line;
while ((line = input.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) return;
    if (line.Length != 3) throw new InvalidOperationException();
    var other = line[0];
    var shouldWin = line[2];
    var match = shouldWin switch // win = 6, draw = 3, lose = 0
        {
            'X' => 0,
            'Y' => 3,
            'Z' => 6,
            _ => throw new InvalidOperationException(),
        } +
        (other, shouldWin) switch // X=A=1, Y=B=2, Z=C=3
        {
            ('A', 'X') => 3,
            ('A', 'Y') => 1,
            ('A', 'Z') => 2,
            ('B', 'X') => 1,
            ('B', 'Y') => 2,
            ('B', 'Z') => 3,
            ('C', 'X') => 2,
            ('C', 'Y') => 3,
            ('C', 'Z') => 1,
            _ => throw new InvalidOperationException(),
        };
    score += match;
}

Console.WriteLine("{0}", score);
