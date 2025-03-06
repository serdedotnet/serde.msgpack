
using MessagePack;

namespace Serde.MsgPack.Tests;

public partial class RoundTripTests
{
    [Fact]
    public void TestChar()
    {
        AssertRoundTrip('c', CharProxy.Instance);
    }

    [Fact]
    public void TestByte()
    {
        AssertRoundTrip((byte)42, ByteProxy.Instance);
        AssertRoundTrip((byte)0xf0, ByteProxy.Instance);
    }

    [Fact]
    public void TestByteSizedUInt()
    {
        AssertRoundTrip(42u, UInt32Proxy.Instance);
    }

    [Fact]
    public void TestPositiveByteSizedInt()
    {
        AssertRoundTrip(42, Int32Proxy.Instance);
    }

    [Fact]
    public void TestNegativeByteSizedInt()
    {
        AssertRoundTrip(-42, Int32Proxy.Instance);
    }

    [Fact]
    public void TestPositiveUInt16()
    {
        AssertRoundTrip((ushort)0x1000, UInt16Proxy.Instance);
    }

    [Fact]
    public void TestNegativeInt16()
    {
        AssertRoundTrip((short)-0x1000, Int16Proxy.Instance);
    }

    [Fact]
    public void TestString()
    {
        AssertRoundTrip("hello", StringProxy.Instance);
    }

    [Fact]
    public void TestNullableString()
    {
        AssertRoundTrip<
            string?,
            NullableRefProxy.Serialize<string, StringProxy>,
            NullableRefProxy.Deserialize<string, StringProxy>>(null);
    }

    [GenerateSerde]
    private enum ByteEnum : byte
    {
        A, B, C
    }

    [Fact]
    public void TestByteEnum()
    {
        AssertRoundTrip(ByteEnum.A, ByteEnumProxy.Instance);
        AssertRoundTrip(ByteEnum.B, ByteEnumProxy.Instance);
        AssertRoundTrip(ByteEnum.C, ByteEnumProxy.Instance);
    }

    [GenerateSerde]
    private enum IntEnum : int
    {
        A, B, C
    }

    [Fact]
    public void TestIntEnum()
    {
        AssertRoundTrip(IntEnum.A, IntEnumProxy.Instance);
        AssertRoundTrip(IntEnum.B, IntEnumProxy.Instance);
        AssertRoundTrip(IntEnum.C, IntEnumProxy.Instance);
    }

    [GenerateSerde]
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
        AssertRoundTrip(new Point { X = 1, Y = 2 });
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

    [Fact(Skip = "Key ordering is not supported for Serde")]
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
        AssertRoundTrip(3.14, DoubleProxy.Instance);
        AssertRoundTrip(double.NaN, DoubleProxy.Instance);
        AssertRoundTrip(double.PositiveInfinity, DoubleProxy.Instance);
    }

    [Fact]
    public void TestArray()
    {
        AssertRoundTrip<
            int[],
            ArrayProxy.Serialize<int, Int32Proxy>,
            ArrayProxy.Deserialize<int, Int32Proxy>>(new[] { 1, 2, 3 });
        AssertRoundTrip<
            string[],
            ArrayProxy.Serialize<string, StringProxy>,
            ArrayProxy.Deserialize<string, StringProxy>>(new[] { "a", "b", "c" });
        AssertRoundTrip<
            Point[],
            ArrayProxy.Serialize<Point, Point>,
            ArrayProxy.Deserialize<Point, Point>>(
                new[] { new Point { X = 1, Y = 2 }, new Point { X = 3, Y = 4 } });
    }

    [Fact]
    public void TestDictionary()
    {
        AssertRoundTrip<
            Dictionary<string, int>,
            DictProxy.Serialize<string, int, StringProxy, Int32Proxy>,
            DictProxy.Deserialize<string, int, StringProxy, Int32Proxy>>(
                new Dictionary<string, int> { { "a", 1 }, { "b", 2 } });
        AssertRoundTrip<
            Dictionary<int, string>,
            DictProxy.Serialize<int, string, Int32Proxy, StringProxy>,
            DictProxy.Deserialize<int, string, Int32Proxy, StringProxy>>(
                new Dictionary<int, string> { { 1, "a" }, { 2, "b" } });
        AssertRoundTrip<
            Dictionary<Point, string>,
            DictProxy.Serialize<Point, string, Point, StringProxy>,
            DictProxy.Deserialize<Point, string, Point, StringProxy>>(
                new Dictionary<Point, string> { { new Point { X = 1, Y = 2 }, "a" }, { new Point { X = 3, Y = 4 }, "b" } });
    }

    [Fact]
    public void TestFloat()
    {
        AssertRoundTrip(3.14f, SingleProxy.Instance);
        AssertRoundTrip(float.NaN, SingleProxy.Instance);
        AssertRoundTrip(float.PositiveInfinity, SingleProxy.Instance);
    }

    private static void AssertRoundTrip<T>(T expected)
        where T : ISerializeProvider<T>, IDeserializeProvider<T>, IEquatable<T>
    {
        AssertRoundTrip(expected, T.SerializeInstance, T.DeserializeInstance);
    }

    private static void AssertRoundTrip<T, TSerialize>(T expected, TSerialize serializeObject)
        where TSerialize : ISerialize<T>, IDeserializeProvider<T>
    {
        AssertRoundTrip(expected, serializeObject, TSerialize.DeserializeInstance);
    }

    private static void AssertRoundTrip<T, TSerialize, TDeserialize>(T expected)
        where TSerialize : ISerializeProvider<T>
        where TDeserialize : IDeserializeProvider<T>
    {
        var serialized = MsgPackSerializer.Serialize(expected, TSerialize.SerializeInstance);
        var actual = MsgPackSerializer.Deserialize<T, IDeserialize<T>>(serialized, TDeserialize.DeserializeInstance);
        Assert.Equal(expected, actual);
    }

    private static void AssertRoundTrip<T, TSerialize, TDeserialize>(T expected, TSerialize serialize, TDeserialize deserialize)
        where TSerialize : ISerialize<T>
        where TDeserialize : IDeserialize<T>
    {
        var serialized = MsgPackSerializer.Serialize(expected, serialize);
        var actual = MsgPackSerializer.Deserialize<T, IDeserialize<T>>(serialized, deserialize);
        Assert.Equal(expected, actual);
    }
}
