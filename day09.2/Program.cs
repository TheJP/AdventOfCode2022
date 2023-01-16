var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

var knots = Enumerable.Range(0, 10).Select(_ => new Position { X = 0, Y = 0 }).ToArray();

var positions = new HashSet<(int, int)>();

foreach (var line in input)
{
    var direction = line[0];
    var count = int.Parse(line.Split()[^1]);

    for (int i = 0; i < count; ++i)
    {
        switch (direction)
        {
            case 'L':
                --knots[0].X;
                break;
            case 'U':
                --knots[0].Y;
                break;
            case 'R':
                ++knots[0].X;
                break;
            case 'D':
                ++knots[0].Y;
                break;
            default:
                throw new InvalidOperationException();
        }

        for (int t = 1; t < knots.Length; ++t)
        {
            var h = knots[t - 1].X - knots[t].X;
            var v = knots[t - 1].Y - knots[t].Y;
            if (Math.Abs(h) > 0 && Math.Abs(v) > 0 && Math.Abs(h) + Math.Abs(v) >= 3)
            {
                knots[t].X += Math.Sign(h);
                knots[t].Y += Math.Sign(v);
            }
            else if (Math.Abs(h) > 1)
            {
                knots[t].X += Math.Sign(h);
            }
            else if (Math.Abs(v) > 1)
            {
                knots[t].Y += Math.Sign(v);
            }
        }

        positions.Add((knots[^1].X, knots[^1].Y));
    }
}

Console.WriteLine("{0}", positions.Count);
