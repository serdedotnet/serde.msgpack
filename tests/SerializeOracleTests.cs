using MessagePack;

namespace Serde.MsgPack.Tests;

/// <summary>
/// Compares the output of the MsgPackSerializer with the output of the MessagePackSerializer.
/// </summary>
public partial class MessagePackOracleTests
{
    [Fact]
    public void TestChar()
    {
        AssertMsgPackEqual('c', new CharWrap());
    }

    [Fact]
    public void TestByteSizedUInt()
    {
        AssertMsgPackEqual(42u, new UInt32Wrap());
    }

    [Fact]
    public void TestPositiveByteSizedInt()
    {
        AssertMsgPackEqual(42, new Int32Wrap());
    }

    [Fact]
    public void TestNegativeByteSizedInt()
    {
        AssertMsgPackEqual(-42, new Int32Wrap());
    }

    [Fact]
    public void TestPositiveUInt16()
    {
        AssertMsgPackEqual((ushort)0x1000, new UInt16Wrap());
    }

    [Fact]
    public void TestNegativeInt16()
    {
        AssertMsgPackEqual((short)-0x1000, new Int16Wrap());
    }

    [Fact]
    public void TestString()
    {
        AssertMsgPackEqual("hello", new StringWrap());
    }

    [Fact]
    public void TestNullableString()
    {
        AssertMsgPackEqual((string?)null, new NullableRefWrap.SerializeImpl<string, StringWrap>());
    }

    [GenerateSerialize]
    private enum ByteEnum : byte
    {
        A, B, C
    }

    [Fact]
    public void TestByteEnum()
    {
        AssertMsgPackEqual(ByteEnum.A, new ByteEnumWrap());
        AssertMsgPackEqual(ByteEnum.B, new ByteEnumWrap());
        AssertMsgPackEqual(ByteEnum.C, new ByteEnumWrap());
    }

    [GenerateSerialize]
    private enum IntEnum : int
    {
        A, B, C
    }

    [Fact]
    public void TestIntEnum()
    {
        AssertMsgPackEqual(IntEnum.A, new IntEnumWrap());
        AssertMsgPackEqual(IntEnum.B, new IntEnumWrap());
        AssertMsgPackEqual(IntEnum.C, new IntEnumWrap());
    }

    [GenerateSerialize]
    [MessagePackObject]
    public partial record Point
    {
        [Key(0)]
        public int X { get; init; }
        [Key(1)]
        public int Y { get; init; }
    }

    [Fact]
    public void TestRecord()
    {
        AssertMsgPackEqual(new Point { X = 1, Y = 2 });
    }

    [GenerateSerialize]
    [MessagePackObject]
    public partial record OutOfOrderKeys
    {
        [Key(1)]
        public int Y { get; init; }
        [Key(0)]
        public int X { get; init; }
    }

    [Fact]
    public void TestOutOfOrderKeys()
    {
        // Out of order keys are not supported for Serde
        Assert.Throws<InvalidOperationException>(() => MsgPackSerializer.Serialize(new OutOfOrderKeys { X = 1, Y = 2 }));

        // Work fine with MessagePack
        var actual = MessagePackSerializer.Serialize(new OutOfOrderKeys { X = 1, Y = 2 });
        Assert.Equal([146, 1, 2], actual);
    }

    [Fact]
    public void TestDouble()
    {
        AssertMsgPackEqual(3.14, new DoubleWrap());
        AssertMsgPackEqual(double.NaN, new DoubleWrap());
        AssertMsgPackEqual(double.PositiveInfinity, new DoubleWrap());
    }

    [Fact]
    public void TestArray()
    {
        AssertMsgPackEqual(new[] { 1, 2, 3 }, new ArrayWrap.SerializeImpl<int, Int32Wrap>());
        AssertMsgPackEqual(new[] { "a", "b", "c" }, new ArrayWrap.SerializeImpl<string, StringWrap>());
        AssertMsgPackEqual(new[] { new Point { X = 1, Y = 2 }, new Point { X = 3, Y = 4 } },
            new ArrayWrap.SerializeImpl<Point, IdWrap<Point>>());
    }

    [Fact]
    public void TestDictionary()
    {
        AssertMsgPackEqual(new Dictionary<string, int> { { "a", 1 }, { "b", 2 } },
            new DictWrap.SerializeImpl<string, StringWrap, int, Int32Wrap>());
        AssertMsgPackEqual(new Dictionary<int, string> { { 1, "a" }, { 2, "b" } },
            new DictWrap.SerializeImpl<int, Int32Wrap, string, StringWrap>());
        AssertMsgPackEqual(new Dictionary<Point, string> { { new Point { X = 1, Y = 2 }, "a" }, { new Point { X = 3, Y = 4 }, "b" } },
            new DictWrap.SerializeImpl<Point, IdWrap<Point>, string, StringWrap>());
    }

    [Fact]
    public void TestFloat()
    {
        AssertMsgPackEqual(3.14f, new SingleWrap());
        AssertMsgPackEqual(float.NaN, new SingleWrap());
        AssertMsgPackEqual(float.PositiveInfinity, new SingleWrap());
    }

    private void AssertMsgPackEqual<T, U>(T value, U proxy)
        where U : ISerialize<T>
    {
        var expected = MessagePackSerializer.Serialize(value);
        var actual = MsgPackSerializer.Serialize(value, proxy);
        Assert.Equal(expected, actual);
    }

    private void AssertMsgPackEqual<T>(T value) where T : ISerialize<T>
        => AssertMsgPackEqual(value, value);

}
