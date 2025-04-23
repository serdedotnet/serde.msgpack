using MessagePack;

namespace Serde.MsgPack.Tests;

/// <summary>
/// Compares the output of the MsgPackSerializer with the output of the MessagePackSerializer.
/// </summary>
public partial class SerializeOracleTests
{
    [Fact]
    public void TestByte()
    {
        AssertMsgPackEqual((byte)42, U8Proxy.Instance);
        AssertMsgPackEqual((byte)0xf0, U8Proxy.Instance);
    }

    [Fact]
    public void TestChar()
    {
        AssertMsgPackEqual('c', CharProxy.Instance);
    }

    [Fact]
    public void TestByteSizedUInt()
    {
        AssertMsgPackEqual(42u, U32Proxy.Instance);
    }

    [Fact]
    public void TestPositiveByteSizedInt()
    {
        AssertMsgPackEqual(42, I32Proxy.Instance);
    }

    [Fact]
    public void TestNegativeByteSizedInt()
    {
        AssertMsgPackEqual(-42, I32Proxy.Instance);
    }

    [Fact]
    public void TestPositiveUInt16()
    {
        AssertMsgPackEqual((ushort)0x1000, U16Proxy.Instance);
    }

    [Fact]
    public void TestNegativeInt16()
    {
        AssertMsgPackEqual((short)-0x1000, I16Proxy.Instance);
    }

    [Fact]
    public void TestString()
    {
        AssertMsgPackEqual("hello", StringProxy.Instance);
    }

    [Fact]
    public void TestNullableString()
    {
        AssertMsgPackEqual((string?)null, NullableRefProxy.Ser<string, StringProxy>.Instance);
    }

    [GenerateSerialize]
    private enum ByteEnum : byte
    {
        A, B, C
    }

    [Fact]
    public void TestByteEnum()
    {
        AssertMsgPackEqual(ByteEnum.A, ByteEnumProxy.Instance);
        AssertMsgPackEqual(ByteEnum.B, ByteEnumProxy.Instance);
        AssertMsgPackEqual(ByteEnum.C, ByteEnumProxy.Instance);
    }

    [GenerateSerialize]
    private enum IntEnum : int
    {
        A, B, C
    }

    [Fact]
    public void TestIntEnum()
    {
        AssertMsgPackEqual(IntEnum.A, IntEnumProxy.Instance);
        AssertMsgPackEqual(IntEnum.B, IntEnumProxy.Instance);
        AssertMsgPackEqual(IntEnum.C, IntEnumProxy.Instance);
    }

    [GenerateSerialize]
    [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
    [MessagePackObject(keyAsPropertyName: true)]
    public partial record Point
    {
        public int X { get; init; }
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

    [Fact(Skip = "Keys are not supported for Serde")]
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
        AssertMsgPackEqual(3.14, F64Proxy.Instance);
        AssertMsgPackEqual(double.NaN, F64Proxy.Instance);
        AssertMsgPackEqual(double.PositiveInfinity, F64Proxy.Instance);
    }

    [Fact]
    public void TestArray()
    {
        AssertMsgPackEqual(new[] { 1, 2, 3 }, ArrayProxy.Ser<int, I32Proxy>.Instance);
        AssertMsgPackEqual(new[] { "a", "b", "c" }, ArrayProxy.Ser<string, StringProxy>.Instance);
        AssertMsgPackEqual(new[] { new Point { X = 1, Y = 2 }, new Point { X = 3, Y = 4 } },
            ArrayProxy.Ser<Point, Point>.Instance);
    }

    [Fact]
    public void TestDictionary()
    {
        AssertMsgPackEqual(new Dictionary<string, int> { { "a", 1 }, { "b", 2 } },
            DictProxy.Ser<string, int, StringProxy, I32Proxy>.Instance);
        AssertMsgPackEqual(new Dictionary<int, string> { { 1, "a" }, { 2, "b" } },
            DictProxy.Ser<int, string, I32Proxy, StringProxy>.Instance);
        AssertMsgPackEqual(new Dictionary<Point, string> { { new Point { X = 1, Y = 2 }, "a" }, { new Point { X = 3, Y = 4 }, "b" } },
            DictProxy.Ser<Point, string, Point, StringProxy>.Instance);
    }

    [Fact]
    public void TestFloat()
    {
        AssertMsgPackEqual(3.14f, F32Proxy.Instance);
        AssertMsgPackEqual(float.NaN, F32Proxy.Instance);
        AssertMsgPackEqual(float.PositiveInfinity, F32Proxy.Instance);
    }

    private void AssertMsgPackEqual<T, U>(T value, U proxy)
        where U : ISerialize<T>
    {
        var expected = MessagePackSerializer.Serialize(value);
        var actual = MsgPackSerializer.Serialize(value, proxy);
        Assert.Equal(expected, actual);
    }

    private void AssertMsgPackEqual<T>(T value) where T : ISerializeProvider<T>
        => AssertMsgPackEqual(value, T.Instance);

}
