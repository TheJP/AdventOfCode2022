using var input = new StreamReader(Environment.GetCommandLineArgs()[1]);
long score = 0;
string? line;
while ((line = input.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) return;
    if (line.Length != 3) throw new InvalidOperationException();
    var other = line[0];
    var self = line[2];
    var match = self - ('X' - 1) +  // X - (X - 1) = 1, Y - (X - 1) = 2, ...
        (other, self) switch // win = 6, draw = 3, lose = 0
        {
            var (x, y) when x == (y - ('X' - 'A')) => 3,
            ('A', 'Y') => 6,
            ('A', 'Z') => 0,
            ('B', 'Z') => 6,
            ('B', 'X') => 0,
            ('C', 'X') => 6,
            ('C', 'Y') => 0,
            _ => throw new InvalidOperationException(),
        };
    score += match;
}

Console.WriteLine("{0}", score);
