class Monkey
{
    public Queue<Worry> Items { get; set; }
    public Func<Worry, Worry> Operation { get; set; }
    public int Test { get; set; }
    public int MonkeyTrue { get; set; }
    public int MonkeyFalse { get; set; }
}

class Worry
{
    public static int[] Divisions { get; set; } = { int.MaxValue };
    public int[] Levels { get; set; }

    private Worry() => Levels = new int[Divisions.Length];

    public Worry(int level) => Levels = Divisions.Select(d => level % d).ToArray();

    public static Worry operator*(Worry lhs, Worry rhs) {
        if (lhs.Levels.Length == 1) lhs = new Worry(lhs.Levels[0]);
        if (rhs.Levels.Length == 1) rhs = new Worry(rhs.Levels[0]);

        var result = new Worry();
        for (int i = 0; i < Divisions.Length; ++i) {
            result.Levels[i] = (lhs.Levels[i] * rhs.Levels[i]) % Divisions[i];
        }

        return result;
    }

    public static Worry operator+(Worry lhs, Worry rhs) {
        if (lhs.Levels.Length == 1) lhs = new Worry(lhs.Levels[0]);
        if (rhs.Levels.Length == 1) rhs = new Worry(rhs.Levels[0]);

        var result = new Worry();
        for (int i = 0; i < Divisions.Length; ++i) {
            result.Levels[i] = (lhs.Levels[i] + rhs.Levels[i]) % Divisions[i];
        }

        return result;
    }

    public static Worry operator*(Worry lhs, int rhs) {
        if (lhs.Levels.Length == 1) lhs = new Worry(lhs.Levels[0]);

        var result = new Worry();
        for (int i = 0; i < Divisions.Length; ++i) {
            result.Levels[i] = (lhs.Levels[i] * (rhs % Divisions[i])) % Divisions[i];
        }

        return result;
    }

    public static Worry operator+(Worry lhs, int rhs) {
        if (lhs.Levels.Length == 1) lhs = new Worry(lhs.Levels[0]);

        var result = new Worry();
        for (int i = 0; i < Divisions.Length; ++i) {
            result.Levels[i] = (lhs.Levels[i] + (rhs % Divisions[i])) % Divisions[i];
        }

        return result;
    }
}
