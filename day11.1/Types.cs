class Monkey
{
    public Queue<int> Items { get;set; }
    public Func<int, int> Operation { get; set; }
    public int Test { get; set; }
    public int MonkeyTrue { get; set; }
    public int MonkeyFalse { get; set; }
}
