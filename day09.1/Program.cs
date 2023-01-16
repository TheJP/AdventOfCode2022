var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

var head = new Position { X = 0, Y = 0 };
var tail = new Position { X = 0, Y = 0 };

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
                --head.X;
                break;
            case 'U':
                --head.Y;
                break;
            case 'R':
                ++head.X;
                break;
            case 'D':
                ++head.Y;
                break;
            default:
                throw new InvalidOperationException();
        }

        var h = head.X - tail.X;
        var v = head.Y - tail.Y;
        if (Math.Abs(h) > 0 && Math.Abs(v) > 0 && Math.Abs(h) + Math.Abs(v) == 3)
        {
            tail.X += Math.Sign(h);
            tail.Y += Math.Sign(v);
        }
        else if (Math.Abs(h) > 1)
        {
            tail.X += Math.Sign(h);
        }
        else if (Math.Abs(v) > 1)
        {
            tail.Y += Math.Sign(v);
        }

        positions.Add((tail.X, tail.Y));
    }
}

Console.WriteLine("{0}", positions.Count);
