using var input = new StreamReader(Environment.GetCommandLineArgs()[1]);
string? line = input.ReadLine();

if (line == null) throw new InvalidOperationException();

// Using chunk.Distinct() would have been simpler.
bool IsStart(string chunk) {
    for (int i = 0; i < chunk.Length - 1; ++i) {
        for (int j = i + 1; j < chunk.Length; ++j) {
            if (chunk[i] == chunk[j]) return false;
        }
    }
    return true;
}

int index = 0;
do
{
    var chunk = line[index..(index + 14)];
    if (IsStart(chunk)) {
        break;
    }
    ++index;
} while (true);

Console.WriteLine("{0}", index + 14);
