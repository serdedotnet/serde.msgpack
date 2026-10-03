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
    public void NullMemberRoundTrip()
    {
        var value = new WithNull { Name = null, Value = 7 };
        var bytes = MsgPackSerializer.Serialize(value);
        Assert.Equal(value, MsgPackSerializer.Deserialize<WithNull>(bytes));
    }
}
