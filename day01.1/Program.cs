using var input = new StreamReader(Environment.GetCommandLineArgs()[1]);
long max = 0;
long currentSum = 0;
string? line;
while ((line = input.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line)) {
        max = Math.Max(max, currentSum);
        currentSum = 0;
    } else {
        currentSum += long.Parse(line);
    }
}

Console.WriteLine("{0}", max);
