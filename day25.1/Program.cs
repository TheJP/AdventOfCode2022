var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

var digits = new Dictionary<char, long>() {
    { '2', 2 },
    { '1', 1 },
    { '0', 0 },
    { '-', -1 },
    { '=', -2 },
};

long sum = 0;
foreach (var line in input)
{
    long number = 0;
    long power = 1;
    for (int i = line.Length - 1; i >= 0; --i)
    {
        number += digits[line[i]] * power;
        power *= 5;
    }
    sum += number;
}

Console.WriteLine($"{sum}");

var result = new List<char>();
bool carry = false;
while (sum > 0 || carry)
{
    var next = sum % 5 + (carry ? 1 : 0);
    sum /= 5;
    carry = false;
    switch (next)
    {
        case 0:
            result.Add('0');
            break;
        case 1:
            result.Add('1');
            break;
        case 2:
            result.Add('2');
            break;
        case 3:
            result.Add('=');
            carry = true;
            break;
        case 4:
            result.Add('-');
            carry = true;
            break;
        case 5:
            result.Add('0');
            carry = true;
            break;
    }
}

result.Reverse();
Console.WriteLine(string.Join(null, result));
