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

Console.WriteLine("Task 1: {0}", SumSizes(root));

// Task 2
const int TotalSpace = 70_000_000;
const int RequiredSpace = 30_000_000;

var usedSpace = root.Size ?? throw new InvalidOperationException();
var freeSpace = TotalSpace - usedSpace;
var deleteAtLeast = RequiredSpace - freeSpace;

var sizes = new List<int>();
void CollectSizes(Folder folder)
{
    int size = folder.Size ?? throw new InvalidOperationException();
    sizes.Add(size);
    foreach (var node in folder.Children.Values)
    {
        if (node is Folder f) CollectSizes(f);
    }
}
CollectSizes(root);

sizes.Sort();
Console.WriteLine("Task 2: {0}", sizes.Where(s => s >= deleteAtLeast).First());
