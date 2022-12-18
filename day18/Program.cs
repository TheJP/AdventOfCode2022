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

Console.WriteLine($"{sides}");
