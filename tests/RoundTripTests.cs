
using MessagePack;

namespace Serde.MsgPack.Tests;

public partial class RoundTripTests
{
    [Fact]
    public void TestBool()
    {
        AssertRoundTrip(true, BoolProxy.Instance);
        AssertRoundTrip(false, BoolProxy.Instance);
    }

    [Fact]
    public void TestChar()
    {
        AssertRoundTrip('c', CharProxy.Instance);
    }

    [Fact]
    public void TestByte()
    {
        AssertRoundTrip((byte)42, U8Proxy.Instance);
        AssertRoundTrip((byte)0xf0, U8Proxy.Instance);
    }

    [Fact]
    public void TestByteSizedUInt()
    {
        AssertRoundTrip(42u, U32Proxy.Instance);
    }

    [Fact]
    public void TestPositiveByteSizedInt()
    {
        AssertRoundTrip(42, I32Proxy.Instance);
    }

    [Fact]
    public void TestNegativeByteSizedInt()
    {
        AssertRoundTrip(-1, I32Proxy.Instance);
        AssertRoundTrip(-32, I32Proxy.Instance);
        AssertRoundTrip(-33, I32Proxy.Instance);
        AssertRoundTrip(-42, I32Proxy.Instance);
        AssertRoundTrip(-128, I32Proxy.Instance);
        AssertRoundTrip(-129, I32Proxy.Instance);
        AssertRoundTrip(-256, I32Proxy.Instance);
        AssertRoundTrip(-257, I32Proxy.Instance);
    }

    [Fact]
    public void TestI64Boundaries()
    {
        AssertRoundTrip(long.MinValue, I64Proxy.Instance);
        AssertRoundTrip(long.MaxValue, I64Proxy.Instance);
        AssertRoundTrip((long)int.MaxValue + 1, I64Proxy.Instance);
        AssertRoundTrip((long)int.MinValue - 1, I64Proxy.Instance);
    }

    [Fact]
    public void TestU64Boundaries()
    {
        AssertRoundTrip(ulong.MaxValue, U64Proxy.Instance);
        AssertRoundTrip((ulong)uint.MaxValue + 1, U64Proxy.Instance);
    }

    [Fact]
    public void TestU32LargeValues()
    {
        AssertRoundTrip(100000u, U32Proxy.Instance);
        AssertRoundTrip(uint.MaxValue, U32Proxy.Instance);
    }

    [Fact]
    public void TestU64LargeValues()
    {
        AssertRoundTrip((ulong)uint.MaxValue + 1, U64Proxy.Instance);
        AssertRoundTrip(ulong.MaxValue, U64Proxy.Instance);
    }

    [Fact]
    public void TestPositiveUInt16()
    {
        AssertRoundTrip((ushort)0x1000, U16Proxy.Instance);
    }

    [Fact]
    public void TestNegativeInt16()
    {
        AssertRoundTrip((short)-0x1000, I16Proxy.Instance);
    }

    [Fact]
    public void TestString()
    {
        AssertRoundTrip("hello", StringProxy.Instance);
    }

    [Fact]
    public void TestLongerString()
    {
        // Exercises the str 8 length-prefixed path (>31 bytes)
        AssertRoundTrip(new string('A', 100), StringProxy.Instance);
    }

    [Fact]
    public void TestNullableString()
    {
        AssertRoundTrip<
            string?,
            NullableRefProxy.Ser<string, StringProxy>,
            NullableRefProxy.De<string, StringProxy>>(null);
    }

    [GenerateSerde]
    private enum ByteEnum : byte
    {
        A, B, C
    }

    [GenerateSerde]
    private enum SparseEnum : byte
    {
        A = 2, B = 3, C = 255
    }

    [Fact]
    public void TestSparseEnum()
    {
        AssertRoundTrip<SparseEnum, SparseEnumProxy, SparseEnumProxy>(SparseEnum.A);
        AssertRoundTrip<SparseEnum, SparseEnumProxy, SparseEnumProxy>(SparseEnum.B);
        AssertRoundTrip<SparseEnum, SparseEnumProxy, SparseEnumProxy>(SparseEnum.C);
    }

    [Fact]
    public void TestByteEnum()
    {
        AssertRoundTrip<ByteEnum, ByteEnumProxy, ByteEnumProxy>(ByteEnum.A);
        AssertRoundTrip<ByteEnum, ByteEnumProxy, ByteEnumProxy>(ByteEnum.B);
        AssertRoundTrip<ByteEnum, ByteEnumProxy, ByteEnumProxy>(ByteEnum.C);
    }

    [GenerateSerde]
    private enum IntEnum : int
    {
        A, B, C
    }

    [Fact]
    public void TestIntEnum()
    {
        AssertRoundTrip<IntEnum, IntEnumProxy, IntEnumProxy>(IntEnum.A);
        AssertRoundTrip<IntEnum, IntEnumProxy, IntEnumProxy>(IntEnum.B);
        AssertRoundTrip<IntEnum, IntEnumProxy, IntEnumProxy>(IntEnum.C);
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
        AssertRoundTrip(3.14, F64Proxy.Instance);
        AssertRoundTrip(double.NaN, F64Proxy.Instance);
        AssertRoundTrip(double.PositiveInfinity, F64Proxy.Instance);
    }

    [Fact]
    public void TestDecimal()
    {
        AssertRoundTrip(0m, DecimalProxy.Instance);
        AssertRoundTrip(123.45m, DecimalProxy.Instance);
        AssertRoundTrip(-9999999999.0001m, DecimalProxy.Instance);
        AssertRoundTrip(decimal.MaxValue, DecimalProxy.Instance);
        AssertRoundTrip(decimal.MinValue, DecimalProxy.Instance);
    }

    [GenerateSerde]
    [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
    [MessagePackObject(keyAsPropertyName: true)]
    public partial record BigRecord
    {
        public int F00 { get; init; }
        public int F01 { get; init; }
        public int F02 { get; init; }
        public int F03 { get; init; }
        public int F04 { get; init; }
        public int F05 { get; init; }
        public int F06 { get; init; }
        public int F07 { get; init; }
        public int F08 { get; init; }
        public int F09 { get; init; }
        public int F10 { get; init; }
        public int F11 { get; init; }
        public int F12 { get; init; }
        public int F13 { get; init; }
        public int F14 { get; init; }
        public int F15 { get; init; }
        public int F16 { get; init; }
        public int F17 { get; init; }
    }

    [Fact]
    public void TestRecordWith18Fields()
    {
        // Exercises the map 16 length path for custom types (>15 fields, format byte 0xde).
        var value = new BigRecord
        {
            F00 = 0, F01 = 1, F02 = 2, F03 = 3, F04 = 4, F05 = 5,
            F06 = 6, F07 = 7, F08 = 8, F09 = 9, F10 = 10, F11 = 11,
            F12 = 12, F13 = 13, F14 = 14, F15 = 15, F16 = 16, F17 = 17,
        };
        AssertRoundTrip(value);
    }

    [Fact]
    public void TestArray()
    {
        AssertRoundTrip<
            int[],
            ArrayProxy.Ser<int, I32Proxy>,
            ArrayProxy.De<int, I32Proxy>>(new[] { 1, 2, 3 });
        AssertRoundTrip<
            string[],
            ArrayProxy.Ser<string, StringProxy>,
            ArrayProxy.De<string, StringProxy>>(new[] { "a", "b", "c" });
        AssertRoundTrip<
            Point[],
            ArrayProxy.Ser<Point, Point>,
            ArrayProxy.De<Point, Point>>(
                new[] { new Point { X = 1, Y = 2 }, new Point { X = 3, Y = 4 } });
    }

    [Fact]
    public void TestArrayWith16Elements()
    {
        // Exercises the array 16 length path (>15 elements, format byte 0xdc)
        var arr = Enumerable.Range(0, 16).ToArray();
        AssertRoundTrip<
            int[],
            ArrayProxy.Ser<int, I32Proxy>,
            ArrayProxy.De<int, I32Proxy>>(arr);
    }

    [Fact]
    public void TestArrayWith300Elements()
    {
        // Exercises the array 16 length path with a multi-byte count (format byte 0xdc)
        var arr = Enumerable.Range(0, 300).ToArray();
        AssertRoundTrip<
            int[],
            ArrayProxy.Ser<int, I32Proxy>,
            ArrayProxy.De<int, I32Proxy>>(arr);
    }

    [Fact]
    public void TestDictionary()
    {
        AssertRoundTrip<
            Dictionary<string, int>,
            DictProxy.Ser<string, int, StringProxy, I32Proxy>,
            DictProxy.De<string, int, StringProxy, I32Proxy>>(
                new Dictionary<string, int> { { "a", 1 }, { "b", 2 } });
        AssertRoundTrip<
            Dictionary<int, string>,
            DictProxy.Ser<int, string, I32Proxy, StringProxy>,
            DictProxy.De<int, string, I32Proxy, StringProxy>>(
                new Dictionary<int, string> { { 1, "a" }, { 2, "b" } });
        AssertRoundTrip<
            Dictionary<Point, string>,
            DictProxy.Ser<Point, string, Point, StringProxy>,
            DictProxy.De<Point, string, Point, StringProxy>>(
                new Dictionary<Point, string> { { new Point { X = 1, Y = 2 }, "a" }, { new Point { X = 3, Y = 4 }, "b" } });
    }

    [Fact]
    public void TestFloat()
    {
        AssertRoundTrip(3.14f, F32Proxy.Instance);
        AssertRoundTrip(float.NaN, F32Proxy.Instance);
        AssertRoundTrip(float.PositiveInfinity, F32Proxy.Instance);
    }

    [Fact]
    public void TestBytes()
    {
        var bytes = new byte[] { 1, 2, 3, 4, 5 };
        AssertRoundTrip(bytes, ByteArrayProxy.Instance);
    }

    [Fact]
    public void TestLargeBytes()
    {
        // Exercises the bin 16 length-prefixed path (>255 bytes, format byte 0xc5)
        var bytes = new byte[300];
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)i;
        }
        AssertRoundTrip(bytes, ByteArrayProxy.Instance);
    }

    [Fact]
    public void TestU128()
    {
        AssertRoundTrip((UInt128)42, U128Proxy.Instance, U128Proxy.Instance);
        AssertRoundTrip(UInt128.MaxValue, U128Proxy.Instance, U128Proxy.Instance);
        AssertRoundTrip(UInt128.Zero, U128Proxy.Instance, U128Proxy.Instance);
    }

    [Fact]
    public void TestI128()
    {
        AssertRoundTrip((Int128)42, I128Proxy.Instance, I128Proxy.Instance);
        AssertRoundTrip((Int128)(-42), I128Proxy.Instance, I128Proxy.Instance);
        AssertRoundTrip(Int128.MaxValue, I128Proxy.Instance, I128Proxy.Instance);
        AssertRoundTrip(Int128.MinValue, I128Proxy.Instance, I128Proxy.Instance);
    }

    [Fact]
    public void TestDateTimeOffset()
    {
        AssertRoundTrip(
            new DateTimeOffset(2024, 6, 7, 1, 2, 3, TimeSpan.FromHours(-7)),
            DateTimeOffsetProxy.Instance, DateTimeOffsetProxy.Instance);
        AssertRoundTrip(
            new DateTimeOffset(1999, 12, 31, 23, 59, 59, TimeSpan.Zero),
            DateTimeOffsetProxy.Instance, DateTimeOffsetProxy.Instance);
    }

    [Fact]
    public void TestDateTime()
    {
        // The timestamp extension stores UTC instants only.
        AssertRoundTrip(
            new DateTime(2024, 6, 7, 1, 2, 3, DateTimeKind.Utc),
            DateTimeProxy.Instance, DateTimeProxy.Instance);
        AssertRoundTrip(
            new DateTime(2024, 6, 7, 1, 2, 3, DateTimeKind.Utc).AddTicks(4567),
            DateTimeProxy.Instance, DateTimeProxy.Instance);
        AssertRoundTrip(
            new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            DateTimeProxy.Instance, DateTimeProxy.Instance);
        AssertRoundTrip(DateTime.UnixEpoch, DateTimeProxy.Instance, DateTimeProxy.Instance);
    }

    [Fact]
    public void TestNonUtcDateTimeRejected()
    {
        // The msgpack timestamp extension only represents UTC instants, so
        // serializing a Local or Unspecified DateTime must throw rather than
        // silently producing ambiguous/non-portable bytes.
        Assert.Throws<InvalidOperationException>(() =>
            MsgPackSerializer.Serialize(
                new DateTime(2024, 6, 7, 1, 2, 3, DateTimeKind.Local), DateTimeProxy.Instance));
        Assert.Throws<InvalidOperationException>(() =>
            MsgPackSerializer.Serialize(
                new DateTime(2024, 6, 7, 1, 2, 3, DateTimeKind.Unspecified), DateTimeProxy.Instance));
    }

    [GenerateSerde]
    [MessagePackObject]
    public partial record CompactPoint
    {
        [SerdeMemberOptions(Ordinal = 0)]
        [Key(0)]
        public int X { get; init; }

        [SerdeMemberOptions(Ordinal = 1)]
        [Key(1)]
        public int Y { get; init; }
    }

    [Fact]
    public void TestCompactRecord()
    {
        AssertRoundTrip(new CompactPoint { X = 1, Y = 2 });
    }

    [GenerateSerde]
    [MessagePackObject]
    public partial record SparseCompact
    {
        [SerdeMemberOptions(Ordinal = 0)]
        [Key(0)]
        public int A { get; init; }

        [SerdeMemberOptions(Ordinal = 2)]
        [Key(2)]
        public int B { get; init; }

        [SerdeMemberOptions(Ordinal = 5)]
        [Key(5)]
        public int C { get; init; }
    }

    [Fact]
    public void TestSparseCompactRecord()
    {
        // Ordinals 0, 2, 5 leave holes at positions 1, 3, 4 (written as nil).
        AssertRoundTrip(new SparseCompact { A = 10, B = 20, C = 30 });
    }

    [GenerateSerde]
    [MessagePackObject]
    public partial record LeadingHoleCompact
    {
        [SerdeMemberOptions(Ordinal = 1)]
        [Key(1)]
        public int V { get; init; }
    }

    [Fact]
    public void TestLeadingHoleCompactRecord()
    {
        // Ordinal 1 leaves a leading hole at position 0 (written as nil).
        AssertRoundTrip(new LeadingHoleCompact { V = 42 });
    }

    [GenerateSerde]
    [MessagePackObject]
    public partial record CompactOuter
    {
        [SerdeMemberOptions(Ordinal = 0)]
        [Key(0)]
        public CompactPoint Inner { get; init; } = new();

        [SerdeMemberOptions(Ordinal = 1)]
        [Key(1)]
        public int Tag { get; init; }
    }

    [Fact]
    public void TestNestedCompactRecord()
    {
        AssertRoundTrip(new CompactOuter { Inner = new CompactPoint { X = 3, Y = 4 }, Tag = 7 });
    }

    [GenerateSerde]
    [MessagePackObject]
    public partial record CompactWithNull
    {
        [SerdeMemberOptions(Ordinal = 0)]
        [Key(0)]
        public string? Name { get; init; }

        [SerdeMemberOptions(Ordinal = 1)]
        [Key(1)]
        public int Value { get; init; }
    }

    [Fact]
    public void TestCompactRecordWithNullField()
    {
        // Exercises the compact SkipValue path: a null field is written as nil.
        AssertRoundTrip(new CompactWithNull { Name = null, Value = 99 });
        AssertRoundTrip(new CompactWithNull { Name = "hi", Value = 99 });
    }

    private static void AssertRoundTrip<T>(T expected)
        where T : ISerializeProvider<T>, IDeserializeProvider<T>, IEquatable<T>
    {
        AssertRoundTrip(expected, SerializeProvider.GetSerialize<T, T>(), DeserializeProvider.GetDeserialize<T, T>());
    }

    private static void AssertRoundTrip<T, TSerialize>(T expected, TSerialize serializeObject)
        where TSerialize : ISerialize<T>, IDeserializeProvider<T>
    {
        AssertRoundTrip(expected, serializeObject, TSerialize.Instance);
    }

    private static void AssertRoundTrip<T, TSerialize, TDeserialize>(T expected)
        where TSerialize : ISerializeProvider<T>
        where TDeserialize : IDeserializeProvider<T>
    {
        var serialized = MsgPackSerializer.Serialize(expected, TSerialize.Instance);
        var actual = MsgPackSerializer.Deserialize<T, IDeserialize<T>>(serialized, TDeserialize.Instance);
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
