interface Node
{
    Folder? Parent { get; }
    int ComputeSize();
}

class Folder : Node
{
    public Folder? Parent { get; set; }
    public Dictionary<string, Node> Children { get; } = new();

    public int? Size { get; private set; } = null;

    public int ComputeSize()
    {
        var size = Children.Values.Select(n => n.ComputeSize()).Sum();
        Size = size;
        return size;
    }
}

class File : Node
{
    public Folder? Parent { get; set; }
    public int Size { get; set; }

    public int ComputeSize() => Size;
}