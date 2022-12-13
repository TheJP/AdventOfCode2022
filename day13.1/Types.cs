enum Ordering
{
    Less,
    Equal,
    Greater,
}

interface Packet
{
    Ordering Ordered(Packet other);
}

record PacketValue(int Value) : Packet
{
    public Ordering Ordered(Packet other)
    {
        if (other is PacketList)
        {
            return new PacketList(new[] { this }).Ordered(other);
        }

        var otherValue = other as PacketValue ?? throw new InvalidOperationException();
        if (Value < otherValue.Value) return Ordering.Less;
        if (Value == otherValue.Value) return Ordering.Equal;
        return Ordering.Greater; /*if (Value > otherValue.Value)*/
    }
}

record PacketList(Packet[] Packets) : Packet
{

    public Ordering Ordered(Packet other)
    {
        if (other is PacketValue)
        {
            return Ordered(new PacketList(new[] { other }));
        }

        var otherPackets = (other as PacketList ?? throw new InvalidOperationException()).Packets;
        for (int i = 0; i < Math.Min(Packets.Length, otherPackets.Length); ++i)
        {
            var ordering = Packets[i].Ordered(otherPackets[i]);
            switch (ordering)
            {
                case Ordering.Less:
                case Ordering.Greater:
                    return ordering;
                default:
                    break;
            }
        }

        if (Packets.Length < otherPackets.Length) return Ordering.Less;
        if (Packets.Length == otherPackets.Length) return Ordering.Equal;
        return Ordering.Greater; /*if (Packets.Length > otherPackets.Length)*/
    }
}