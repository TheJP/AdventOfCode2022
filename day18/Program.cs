var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);
(int x, int y, int z)[] perm =
{
    (0, 0, 1), (0, 0, -1),
    (0, 1, 0), (0, -1, 0),
    (1, 0, 0), (-1, 0, 0),
};

var cubes = new HashSet<(int x, int y, int z)>();
int sides = 0;
foreach (var line in input)
{
    var parts = line.Split(',').Select(int.Parse).ToArray();
    var cube = (x: parts[0], y: parts[1], z: parts[2]);
    if (cubes.Contains(cube)) continue;

    sides += 6;
    foreach (var p in perm)
    {
        if (cubes.Contains((cube.x + p.x, cube.y + p.y, cube.z + p.z))) sides -= 2;
    }

    cubes.Add(cube);
}

Console.WriteLine($"Task 1: {sides}");

var min = (
    x: cubes.Select(c => c.x).Min() - 1,
    y: cubes.Select(c => c.y).Min() - 1,
    z: cubes.Select(c => c.z).Min() - 1
);
var max = (
    x: cubes.Select(c => c.x).Max() + 1,
    y: cubes.Select(c => c.y).Max() + 1,
    z: cubes.Select(c => c.z).Max() + 1
);

var bfs = new Queue<(int x, int y, int z)>();
var visited = new HashSet<(int, int, int)>();
bfs.Enqueue(min);

while (bfs.Count > 0)
{
    var current = bfs.Dequeue();
    foreach (var p in perm)
    {
        var neighbour = (x: current.x + p.x, y: current.y + p.y, z: current.z + p.z);
        if (!cubes.Contains(neighbour) && !visited.Contains(neighbour) &&
            min.x <= neighbour.x && neighbour.x <= max.x &&
            min.y <= neighbour.y && neighbour.y <= max.y &&
            min.z <= neighbour.z && neighbour.z <= max.z)
        {
            bfs.Enqueue(neighbour);
            visited.Add(neighbour);
        }
    }
}

var sidesTask2 = sides;

for (int z = min.z; z <= max.z; ++z)
{
    for (int y = min.y; y <= max.y; ++y)
    {
        for (int x = min.x; x <= max.x; ++x)
        {
            var cube = (x, y, z);
            if (!cubes.Contains(cube) && !visited.Contains(cube)) {
                foreach (var p in perm)
                {
                    if (cubes.Contains((cube.x + p.x, cube.y + p.y, cube.z + p.z))) --sidesTask2;
                }
            }
        }
    }
}

Console.WriteLine($"Task 2: {sidesTask2}");
