using System.Text.RegularExpressions;

var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

// Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore.
// Each obsidian robot costs 3 ore and 14 clay.
// Each geode robot costs 2 ore and 7 obsidian.
var regex = new Regex("Blueprint [0-9]+: Each ore robot costs ([0-9]+) ore. Each clay robot costs ([0-9]+) ore. Each obsidian robot costs ([0-9]+) ore and ([0-9]+) clay. Each geode robot costs ([0-9]+) ore and ([0-9]+) obsidian.");

int score = 1;
int blueprint = 1;
foreach (var line in input.Take(3))
{
    Console.WriteLine($"Blueprint: {blueprint}");

    var match = regex.Match(line);
    if (!match.Success) throw new InvalidOperationException(line);

    var oreForOre = byte.Parse(match.Groups[1].Value);
    var oreForClay = byte.Parse(match.Groups[2].Value);
    var oreForObsidian = byte.Parse(match.Groups[3].Value);
    var clayForObsidian = byte.Parse(match.Groups[4].Value);
    var oreForGeode = byte.Parse(match.Groups[5].Value);
    var obsidianForGeode = byte.Parse(match.Groups[6].Value);

    byte maxOre = Math.Max(Math.Max(oreForOre, oreForClay), Math.Max(oreForObsidian, oreForGeode));

    var realities = new HashSet<State>();
    realities.Add(new State(new Res(0, 0, 0, 0), new Res(1, 0, 0, 0)));

    for (int day = 1; day < 32; ++day)
    {
        Console.WriteLine($"Realities on day {day}: {realities.Count}");
        var oldRealities = realities;
        realities = new();
        foreach (var reality in oldRealities)
        {
            var done = (
                ore: reality.robots.ore >= maxOre,
                clay: reality.robots.clay >= clayForObsidian,
                obsidian: reality.robots.obsidian >= obsidianForGeode
            );
            var next = new State(
                new Res(
                    done.ore ? byte.MaxValue : (byte)(reality.store.ore + reality.robots.ore),
                    done.clay ? byte.MaxValue : (byte)(reality.store.clay + reality.robots.clay),
                    done.obsidian ? byte.MaxValue : (byte)(reality.store.obsidian + reality.robots.obsidian),
                    (byte)(reality.store.geode)
                ),
                reality.robots
            );

            if (reality.store.ore >= oreForGeode && reality.store.obsidian >= obsidianForGeode)
            {
                realities.Add(new State(
                    new Res(done.ore ? byte.MaxValue : (byte)(next.store.ore - oreForGeode), next.store.clay, done.obsidian ? byte.MaxValue : (byte)(next.store.obsidian - obsidianForGeode), (byte)(next.store.geode + (32 - day))),
                    new Res(next.robots.ore, next.robots.clay, next.robots.obsidian, 0)
                ));
            }

            if (reality.robots.ore >= maxOre && reality.robots.obsidian >= obsidianForGeode)
            {
                continue;
            }

            realities.Add(next);
            if (reality.store.ore >= oreForOre && reality.robots.ore < maxOre)
            {
                realities.Add(new State(
                    new Res(done.ore ? byte.MaxValue : (byte)(next.store.ore - oreForOre), next.store.clay, next.store.obsidian, next.store.geode),
                    new Res((byte)(next.robots.ore + 1), next.robots.clay, next.robots.obsidian, next.robots.geode)
                ));
            }
            if (reality.store.ore >= oreForClay && reality.robots.clay < clayForObsidian)
            {
                realities.Add(new State(
                    new Res(done.ore ? byte.MaxValue : (byte)(next.store.ore - oreForClay), next.store.clay, next.store.obsidian, next.store.geode),
                    new Res(next.robots.ore, (byte)(next.robots.clay + 1), next.robots.obsidian, next.robots.geode)
                ));
            }
            if (reality.store.ore >= oreForObsidian && reality.store.clay >= clayForObsidian && reality.robots.obsidian < obsidianForGeode)
            {
                realities.Add(new State(
                    new Res(done.ore ? byte.MaxValue : (byte)(next.store.ore - oreForObsidian), done.clay ? byte.MaxValue : (byte)(next.store.clay - clayForObsidian), next.store.obsidian, next.store.geode),
                    new Res(next.robots.ore, next.robots.clay, (byte)(next.robots.obsidian + 1), next.robots.geode)
                ));
            }
        }
    }

    int geodes = realities.Select(r => r.store.geode).Max();
    Console.WriteLine($"Geodes: {geodes}");
    Console.WriteLine();

    score *= geodes;
    ++blueprint;
}

Console.WriteLine($"Result: {score}");
