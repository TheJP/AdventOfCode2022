
using System.Text.RegularExpressions;
var regex = new Regex("Valve ([A-Z]+) has flow rate=([0-9]+); tunnels? leads? to valves? ([A-Z, ]+)");

var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

const int limit = 26;
const string start = "AA";

var graph = new Dictionary<string, (int key, int flow, string[] next)>();

int key = 0;
foreach (var line in input)
{
    var match = regex.Match(line);
    if (!match.Success) throw new InvalidOperationException();

    var node = match.Groups[1].Value;
    var flow = int.Parse(match.Groups[2].Value);
    var next = match.Groups[3].Value.Trim().Split(", ");

    var id = flow > 0 || node == start ? key++ : -1;
    graph.Add(node, (id, flow, next));
}

var count = key;
var distance = new int[count, count];
foreach (var node in graph)
{
    var visited = new HashSet<string>();
    var bfs = new Queue<(int depth, string node)>();
    bfs.Enqueue((0, node.Key));
    visited.Add(node.Key);

    while (bfs.Count > 0)
    {
        var current = bfs.Dequeue();
        if (node.Value.key >= 0 && graph[current.node].key >= 0)
        {
            distance[node.Value.key, graph[current.node].key] = current.depth;
        }

        foreach (var next in graph[current.node].next)
        {
            if (!visited.Contains(next))
            {
                visited.Add(next);
                bfs.Enqueue((current.depth + 1, next));
            }
        }
    }
}

// for (int i = 0; i < count; ++i)
// {
//     for (int j = 0; j < count; ++j)
//     {
//         Console.Write($"{distance[i, j]} ");
//     }
//     Console.WriteLine();
// }

var flows = new int[count];
foreach (var node in graph.Values)
{
    if (node.key >= 0) flows[node.key] = node.flow;
}

int FindMax(int timeLeft, int node, int timeLeftE, int nodeE, int opened)
{
    if (timeLeftE > timeLeft)
    {
        (timeLeft, timeLeftE) = (timeLeftE, timeLeft);
        (node, nodeE) = (nodeE, node);
    }

    if (timeLeft <= 0) return 0;

    var best = 0;
    // var bestTime = 0;

    for (int i = 0; i < count; ++i)
    {
        if (i == node || (opened & (1 << i)) > 0) continue;

        var newTime = timeLeft - 1 - distance[i, node];

        if (newTime < 0) continue;

        var newFlow = newTime * flows[i] + FindMax(newTime, i, timeLeftE, nodeE, opened | (1 << i));
        best = Math.Max(best, newFlow);
    }

    return best;
}

var score = FindMax(limit, graph[start].key, limit, graph[start].key, 0);
Console.WriteLine($"{score}");
