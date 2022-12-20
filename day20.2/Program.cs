var input = File.ReadLines(Environment.GetCommandLineArgs()[1])
    .Select(long.Parse)
    .Select((n, i) => (number: n * 811589153, index: i))
    .ToArray();
var index = Enumerable.Range(0, input.Length).ToArray();

// Console.WriteLine("Initial");
// foreach (var n in input)
// {
//     Console.Write($"{n.number} ");
// }
// Console.WriteLine();
// Console.WriteLine();

for (int round = 1; round <= 10; ++round)
{
    for (int i = 0; i < input.Length; ++i)
    {
        var current = index[i];
        var x = input[current];
        var number = (int)(x.number % (input.Length - 1));
        // Console.WriteLine($"{x.number} -> {number}");
        for (int j = number; j > 0; --j)
        {
            var next = current + 1;
            if (next >= input.Length)
            {
                var rightmost = input[input.Length - 1];
                input = input.Take(input.Length - 1).Prepend(rightmost).ToArray();
                for (int k = 0; k < index.Length; ++k)
                {
                    ++index[k];
                }
                current = 0;
                next = current + 1;
                index[rightmost.index] = 0;
            }

            var tmp = input[next];
            input[next] = input[current];
            input[current] = tmp;
            index[tmp.index] = current;
            current = next;
        }

        for (int j = number; j < 0; ++j)
        {
            var next = current - 1;
            if (next < 0)
            {
                var leftmost = input[0];
                input = input.Skip(1).Append(leftmost).ToArray();
                for (int k = 0; k < index.Length; ++k)
                {
                    --index[k];
                }
                current = input.Length - 1;
                next = current - 1;
                index[leftmost.index] = input.Length - 1;
            }

            var tmp = input[next];
            input[next] = input[current];
            input[current] = tmp;
            index[tmp.index] = current;
            current = next;
        }

        index[x.index] = current;
    }

    // Console.WriteLine($"After round {round}");
    // foreach (var n in input)
    // {
    //     Console.Write($"{n.number} ");
    // }
    // Console.WriteLine();
    // Console.WriteLine();
}

var zeroIndex = index[input.First(x => x.number == 0).index];
Console.WriteLine($"{input[(zeroIndex + 1000) % input.Length]} {input[(zeroIndex + 2000) % input.Length]} {input[(zeroIndex + 3000) % input.Length]}");
Console.WriteLine($"{input[(zeroIndex + 1000) % input.Length].number + input[(zeroIndex + 2000) % input.Length].number + input[(zeroIndex + 3000) % input.Length].number}");
