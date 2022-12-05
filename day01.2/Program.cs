using var input = new StreamReader(Environment.GetCommandLineArgs()[1]);
var sums = new List<long>();
long currentSum = 0;
string? line;
while ((line = input.ReadLine()) != null)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        sums.Add(currentSum);
        currentSum = 0;
    }
    else
    {
        currentSum += long.Parse(line);
    }
}

sums.Sort();

var topThree = sums.ToArray()[^3..];
Console.WriteLine("{0}", string.Join(", ", topThree));
Console.WriteLine("{0}", topThree.Sum());
