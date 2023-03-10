using System.Text.RegularExpressions;

var input = File.ReadLines(Environment.GetCommandLineArgs()[1]);

// Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore.
// Each obsidian robot costs 3 ore and 14 clay.
// Each geode robot costs 2 ore and 7 obsidian.
var regex = new Regex("Blueprint [0-9]+: Each ore robot costs ([0-9]+) ore. Each clay robot costs ([0-9]+) ore. Each obsidian robot costs ([0-9]+) ore and ([0-9]+) clay. Each geode robot costs ([0-9]+) ore and ([0-9]+) obsidian.");

int score = 0;
int blueprint = 1;
foreach (var line in input)
{
    var match = regex.Match(line);
    if (!match.Success) throw new InvalidOperationException(line);

    var oreForOre = int.Parse(match.Groups[1].Value);
    var oreForClay = int.Parse(match.Groups[2].Value);
    var oreForObsidian = int.Parse(match.Groups[3].Value);
    var clayForObsidian = int.Parse(match.Groups[4].Value);
    var oreForGeode = int.Parse(match.Groups[5].Value);
    var obsidianForGeode = int.Parse(match.Groups[6].Value);

    // var memoization = new int[26, 100, 100, 100, 26, 26, 26, 26]; <- too large
    var memoization = new Dictionary<(int, int, int, int, int, int, int, int), int>();

    int MaxGeodes(int minute, int ore, int clay, int obsidian, int oreRobot, int clayRobot, int obsidianRobot, int geodeRobot)
    {
        if (minute > 24) return 0;
        var key = (minute, ore, clay, obsidian, oreRobot, clayRobot, obsidianRobot, geodeRobot);
        if (memoization.ContainsKey(key)) return memoization[key];

        var newOre = ore + oreRobot;
        var newClay = clay + clayRobot;
        var newObsidian = obsidian + obsidianRobot;
        var best = MaxGeodes(minute + 1, newOre, newClay, newObsidian, oreRobot, clayRobot, obsidianRobot, geodeRobot);
        if (ore >= oreForOre) best = Math.Max(best, MaxGeodes(minute + 1, newOre - oreForOre, newClay, newObsidian, oreRobot + 1, clayRobot, obsidianRobot, geodeRobot));
        if (ore >= oreForClay) best = Math.Max(best, MaxGeodes(minute + 1, newOre - oreForClay, newClay, newObsidian, oreRobot, clayRobot + 1, obsidianRobot, geodeRobot));
        if (ore >= oreForObsidian && clay >= clayForObsidian) best = Math.Max(best, MaxGeodes(minute + 1, newOre - oreForObsidian, newClay - clayForObsidian, newObsidian, oreRobot, clayRobot, obsidianRobot + 1, geodeRobot));
        if (ore >= oreForGeode && obsidian >= obsidianForGeode) best = Math.Max(best, MaxGeodes(minute + 1, newOre - oreForGeode, newClay, newObsidian - obsidianForGeode, oreRobot, clayRobot, obsidianRobot, geodeRobot + 1));

        var result = geodeRobot + best;
        memoization.Add(key, result);
        return result;
    }

    var geodes = MaxGeodes(1, 0, 0, 0, 1, 0, 0, 0);
    score += blueprint * geodes;
    ++blueprint;
}

Console.WriteLine(score);
