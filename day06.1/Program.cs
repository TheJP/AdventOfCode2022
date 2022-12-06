using var input = new StreamReader(Environment.GetCommandLineArgs()[1]);
string? line = input.ReadLine();

if (line == null) throw new InvalidOperationException();

bool IsStart(string four) {
    for (int i = 0; i < four.Length - 1; ++i) {
        for (int j = i + 1; j < four.Length; ++j) {
            if (four[i] == four[j]) return false;
        }
    }
    return true;
}

int index = 0;
do
{
    var four = line[index..(index + 4)];
    if (IsStart(four)) {
        break;
    }
    ++index;
} while (true);

Console.WriteLine("{0}", index + 4);
