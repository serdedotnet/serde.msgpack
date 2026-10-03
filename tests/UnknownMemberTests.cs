namespace Serde.MsgPack.Tests;

public partial class UnknownMemberTests
{
    [GenerateSerde]
    public partial record WithNull
    {
        public string? Name { get; init; }
        public int Value { get; init; }
    }

    [Fact]
    public void NullMemberIsOmitted()
    {
        var value = new WithNull { Name = null, Value = 7 };
        var bytes = MsgPackSerializer.Serialize(value);
        Assert.Equal(0x81, bytes[0]); // a map with one entry
        Assert.Equal(value, MsgPackSerializer.Deserialize<WithNull>(bytes));
    }

    // A member of every MessagePack shape, so that skipping them as unknown members covers each
    [GenerateSerde]
    public partial record Wide
    {
        public string? Null { get; init; }
        public bool Flag { get; init; }
        public long Big { get; init; }
        public int Negative { get; init; }
        public double Number { get; init; }
        public string Text { get; init; } = "";
        public byte[] Bytes { get; init; } = [];
        public List<int> List { get; init; } = [];
        public Dictionary<string, int> Map { get; init; } = [];
        public WithNull Nested { get; init; } = new();
        public int A { get; init; }
        public string Tail { get; init; } = "";
    }

    [GenerateSerde]
    public partial record Narrow
    {
        public int A { get; init; }
    }

    [GenerateSerde]
    [SerdeTypeOptions(DenyUnknownMembers = true)]
    public partial record DenyNarrow
    {
        public int A { get; init; }
    }

    [GenerateSerde]
    public partial record WithOptional
    {
        public int A { get; init; }
        public string? Name { get; init; }
    }

    [GenerateSerde]
    public partial record OuterWide
    {
        public Wide Inner { get; init; } = new();
        public int After { get; init; }
    }

    [GenerateSerde]
    public partial record OuterNarrow
    {
        public Narrow Inner { get; init; } = new();
        public int After { get; init; }
    }

    private static readonly Wide s_wide = new()
    {
        Flag = true,
        Big = long.MaxValue,
        Negative = -1000,
        Number = 1.5,
        Text = new string('x', 40),
        Bytes = [1, 2, 3],
        List = [1, 2],
        Map = new() { ["k"] = 1 },
        Nested = new WithNull { Name = "n", Value = 1 },
        A = 42,
        Tail = "t",
    };

    [Fact]
    public void UnknownMembersAreSkipped()
    {
        var narrow = MsgPackSerializer.Deserialize<Narrow>(MsgPackSerializer.Serialize(s_wide));
        Assert.Equal(42, narrow.A);
    }

    [Fact]
    public void SkippingConsumesExactlyTheUnknownMembers()
    {
        var bytes = MsgPackSerializer.Serialize(new OuterWide { Inner = s_wide, After = 9 });
        var outer = MsgPackSerializer.Deserialize<OuterNarrow>(bytes);
        Assert.Equal(42, outer.Inner.A);
        Assert.Equal(9, outer.After);
    }

    [Fact]
    public void OmittedMembersAreAllowed()
    {
        var bytes = MsgPackSerializer.Serialize(new Narrow { A = 1 });
        Assert.Equal(
            new WithOptional { A = 1, Name = null },
            MsgPackSerializer.Deserialize<WithOptional>(bytes)
        );
    }

    [Fact]
    public void DeniedUnknownMemberThrows()
    {
        var bytes = MsgPackSerializer.Serialize(s_wide);
        Assert.Throws<DeserializeException>(() => MsgPackSerializer.Deserialize<DenyNarrow>(bytes));
    }

    [Fact]
    public void WrongShapeThrowsDeserializeException()
    {
        var bytes = MsgPackSerializer.Serialize("not a map", StringProxy.Instance);
        Assert.Throws<DeserializeException>(() => MsgPackSerializer.Deserialize<Narrow>(bytes));
    }

    [Fact]
    public void TruncatedInputThrowsDeserializeException()
    {
        var bytes = MsgPackSerializer.Serialize(new Narrow { A = 1 });
        Assert.Throws<DeserializeException>(() =>
            MsgPackSerializer.Deserialize<Narrow>(bytes[..^1])
        );
    }
}
