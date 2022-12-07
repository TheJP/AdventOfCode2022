using var input = new StreamReader(Environment.GetCommandLineArgs()[1]);
const int MaxSize = 100_000;

var root = new Folder { Parent = null };
var currentFolder = root;

string? line;
while ((line = input.ReadLine()) != null)
{
    if (line.StartsWith("$ cd"))
    {
        if (line == "$ cd /") continue; // only has on `cd /` at the start
        if (line == "$ cd ..") currentFolder = currentFolder.Parent ?? throw new InvalidOperationException();
        else
        {
            var folderName = line.Split()[^1];
            currentFolder = currentFolder.Children[folderName] switch
            {
                Folder f => f,
                _ => throw new InvalidOperationException(),
            };
        }
    }
    else if (line == "$ ls")
    {
        if (currentFolder.Children.Count > 0) throw new InvalidOperationException();
    }
    else if (line.StartsWith("dir"))
    {
        var folderName = line.Split()[^1];
        currentFolder.Children.Add(folderName, new Folder { Parent = currentFolder });
    }
    else
    {
        var fileLine = line.Split();
        var fileSize = int.Parse(fileLine[0]);
        var fileName = fileLine[1];
        currentFolder.Children.Add(fileName, new File { Parent = currentFolder, Size = fileSize });
    }
}

root.ComputeSize();

int SumSizes(Folder folder)
{
    int size = folder.Size ?? throw new InvalidOperationException();
    size = size <= MaxSize ? size : 0;
    return size + folder.Children.Values.Select(n => n is Folder f ? SumSizes(f) : 0).Sum();
}

Console.WriteLine("{0}", SumSizes(root));
