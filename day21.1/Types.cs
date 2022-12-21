interface Node
{
    long ComputeValue(Dictionary<string, Node> tree);
}

class ValueNode : Node
{
    public long Value { get; set; }
    public long ComputeValue(Dictionary<string, Node> tree) => Value;
}

class Operation : Node
{
    public string A { get; set; } = string.Empty;
    public char Operator { get; set; }
    public string B { get; set; } = string.Empty;

    public long ComputeValue(Dictionary<string, Node> tree)
    {
        var a = tree[A].ComputeValue(tree);
        var b = tree[B].ComputeValue(tree);
        return Operator switch
        {
            '+' => a + b,
            '-' => a - b,
            '*' => a * b,
            '/' => a / b,
            _ => throw new InvalidOperationException(),
        };
    }
}
