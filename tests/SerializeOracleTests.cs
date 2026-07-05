using MessagePack;

namespace Serde.MsgPack.Tests;

/// <summary>
/// Compares the output of the MsgPackSerializer with the output of the MessagePackSerializer.
/// </summary>
public partial class SerializeOracleTests
{
    [Fact]
    public void TestBool()
    {
        AssertMsgPackEqual(true, BoolProxy.Instance);
        AssertMsgPackEqual(false, BoolProxy.Instance);
    }

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
        // Enum variant names are emitted using serde's default member format (camelCase),
        // so single-letter names are lowercased on the wire.
        AssertEnumName(ByteEnum.A, "a", SerializeProvider.GetSerialize<ByteEnum, ByteEnumProxy>());
        AssertEnumName(ByteEnum.B, "b", SerializeProvider.GetSerialize<ByteEnum, ByteEnumProxy>());
        AssertEnumName(ByteEnum.C, "c", SerializeProvider.GetSerialize<ByteEnum, ByteEnumProxy>());
    }

    [GenerateSerialize]
    private enum IntEnum : int
    {
        A, B, C
    }

    [Fact]
    public void TestIntEnum()
    {
        AssertEnumName(IntEnum.A, "a", SerializeProvider.GetSerialize<IntEnum, IntEnumProxy>());
        AssertEnumName(IntEnum.B, "b", SerializeProvider.GetSerialize<IntEnum, IntEnumProxy>());
        AssertEnumName(IntEnum.C, "c", SerializeProvider.GetSerialize<IntEnum, IntEnumProxy>());
    }

    // With `As = <underlying>`, the enum is serialized as its underlying integral value via the
    // normal primitive path, which matches MessagePack-CSharp's numeric enum encoding.
    [GenerateSerialize(AsUnderlying = true)]
    private enum AsByteEnum : byte
    {
        A = 1, B = 2, C = 3
    }

    [Fact]
    public void TestAsByteEnum()
    {
        AssertMsgPackEqual(AsByteEnum.A, SerializeProvider.GetSerialize<AsByteEnum, AsByteEnumProxy>());
        AssertMsgPackEqual(AsByteEnum.B, SerializeProvider.GetSerialize<AsByteEnum, AsByteEnumProxy>());
        AssertMsgPackEqual(AsByteEnum.C, SerializeProvider.GetSerialize<AsByteEnum, AsByteEnumProxy>());
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

    [Fact]
    public void TestBytes()
    {
        AssertMsgPackEqual(new byte[] { 1, 2, 3, 4, 5 }, ByteArrayProxy.Instance);
    }

    [Fact]
    public void TestLargeBytes()
    {
        // Validates the bin 16 length-prefixed path (>255 bytes) against the reference library
        var bytes = new byte[300];
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = (byte)i;
        }
        AssertMsgPackEqual(bytes, ByteArrayProxy.Instance);
    }

    [Fact]
    public void TestDecimal()
    {
        // MessagePack serializes decimal as its invariant string form; verify we match.
        AssertMsgPackEqual(123.45m, DecimalProxy.Instance);
        AssertMsgPackEqual(-9999999999.0001m, DecimalProxy.Instance);
        AssertMsgPackEqual(decimal.MaxValue, DecimalProxy.Instance);
    }

    [GenerateSerialize]
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
        AssertMsgPackEqual(new BigRecord
        {
            F00 = 0, F01 = 1, F02 = 2, F03 = 3, F04 = 4, F05 = 5,
            F06 = 6, F07 = 7, F08 = 8, F09 = 9, F10 = 10, F11 = 11,
            F12 = 12, F13 = 13, F14 = 14, F15 = 15, F16 = 16, F17 = 17,
        });
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
        // Compact (ordinal) types must be byte-identical to MessagePack's
        // default integer-keyed array encoding.
        AssertMsgPackEqual(new CompactPoint { X = 1, Y = 2 });
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
        // Holes (positions 1, 3, 4) must be nil-filled exactly as MessagePack does.
        AssertMsgPackEqual(new SparseCompact { A = 10, B = 20, C = 30 });
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
        AssertMsgPackEqual(new CompactWithNull { Name = null, Value = 99 });
    }

    [Fact]
    public void TestDateTime()
    {
        // timestamp 32 (whole seconds, post-epoch)
        AssertMsgPackEqual(new DateTime(2024, 6, 7, 1, 2, 3, DateTimeKind.Utc), DateTimeProxy.Instance);
        // timestamp 64 (sub-second nanoseconds)
        AssertMsgPackEqual(new DateTime(2024, 6, 7, 1, 2, 3, DateTimeKind.Utc).AddTicks(4567), DateTimeProxy.Instance);
        // timestamp 96 (pre-1970, negative seconds)
        AssertMsgPackEqual(new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc), DateTimeProxy.Instance);
        // epoch
        AssertMsgPackEqual(DateTime.UnixEpoch, DateTimeProxy.Instance);
    }

    [Fact]
    public void TestDateTimeOffset()
    {
        AssertMsgPackEqual(
            new DateTimeOffset(2024, 6, 7, 1, 2, 3, TimeSpan.FromHours(-7)),
            DateTimeOffsetProxy.Instance);
        AssertMsgPackEqual(
            new DateTimeOffset(1999, 12, 31, 23, 59, 59, TimeSpan.Zero),
            DateTimeOffsetProxy.Instance);
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

    // Enums are serialized as a msgpack string of the variant name (serde's name-based
    // model), which intentionally differs from MessagePack-CSharp's numeric enum encoding.
    // Verify the bytes match the standard msgpack string encoding of the variant name.
    private void AssertEnumName<T, U>(T value, string name, U proxy)
        where U : ISerialize<T>
    {
        var expected = MessagePackSerializer.Serialize(name);
        var actual = MsgPackSerializer.Serialize(value, proxy);
        Assert.Equal(expected, actual);
    }

}
