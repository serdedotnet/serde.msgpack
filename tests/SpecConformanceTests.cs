namespace Serde.MsgPack.Tests;

/// <summary>
/// Conformance tests that pin the exact byte encoding produced and accepted for
/// each MessagePack format family.
/// See the MessagePack specification: https://github.com/msgpack/msgpack/blob/master/spec.md
///
/// Each vector is expressed as a hex string and exercises both directions
/// (serialize produces the bytes, deserialize reconstructs the value) unless
/// otherwise noted.
///
/// Features not yet supported by the library are noted but excluded:
/// - The ext family (fixext / ext 8 / ext 16 / ext 32, format bytes 0xc7–0xc9, 0xd4–0xd8)
/// - The msgpack timestamp extension (ext type -1); DateTime/DateTimeOffset are
///   encoded as ISO-8601 strings instead.
/// </summary>
public partial class SpecConformanceTests
{
    // ── Positive integers (fixint + uint 8/16/32/64) ──────────────
    // Non-negative values are always encoded in the smallest unsigned form.

    [Theory]
    [InlineData("00", 0UL)] // positive fixint
    [InlineData("01", 1UL)] // positive fixint
    [InlineData("17", 23UL)] // positive fixint
    [InlineData("7f", 127UL)] // max positive fixint
    [InlineData("cc80", 128UL)] // uint 8
    [InlineData("ccff", 255UL)] // uint 8
    [InlineData("cd0100", 256UL)] // uint 16
    [InlineData("cdffff", 65535UL)] // uint 16
    [InlineData("ce00010000", 65536UL)] // uint 32
    [InlineData("ce000f4240", 1000000UL)] // uint 32
    [InlineData("ceffffffff", 4294967295UL)] // uint 32
    [InlineData("cf0000000100000000", 4294967296UL)] // uint 64
    [InlineData("cfffffffffffffffff", 18446744073709551615UL)] // uint 64
    public void UnsignedInteger(string hex, ulong expected)
    {
        AssertRoundTrip(hex, expected, U64Proxy.Instance);
    }

    // ── Negative integers (negative fixint + int 8/16/32/64) ──────

    [Theory]
    [InlineData("ff", -1L)] // negative fixint
    [InlineData("e0", -32L)] // min negative fixint
    [InlineData("d0df", -33L)] // int 8
    [InlineData("d080", -128L)] // int 8
    [InlineData("d1ff7f", -129L)] // int 16
    [InlineData("d18000", -32768L)] // int 16
    [InlineData("d2ffff7fff", -32769L)] // int 32
    [InlineData("d280000000", -2147483648L)] // int 32
    [InlineData("d3ffffffff7fffffff", -2147483649L)] // int 64
    [InlineData("d38000000000000000", -9223372036854775808L)] // int 64 (long.MinValue)
    public void NegativeInteger(string hex, long expected)
    {
        AssertRoundTrip(hex, expected, I64Proxy.Instance);
    }

    // ── Booleans (0xc2 / 0xc3) ────────────────────────────────────

    [Theory]
    [InlineData("c2", false)]
    [InlineData("c3", true)]
    public void Boolean(string hex, bool expected)
    {
        AssertRoundTrip(hex, expected, BoolProxy.Instance);
    }

    // ── Nil (0xc0) ────────────────────────────────────────────────

    [Fact]
    public void Nil()
    {
        var bytes = Convert.FromHexString("c0");
        var actual = MsgPackSerializer.Deserialize<string?, IDeserialize<string?>>(
            bytes,
            NullableRefProxy.De<string, StringProxy>.Instance
        );
        Assert.Null(actual);

        var serialized = MsgPackSerializer.Serialize<string?>(
            null,
            NullableRefProxy.Ser<string, StringProxy>.Instance
        );
        Assert.Equal("C0", Convert.ToHexString(serialized));
    }

    // ── Strings (fixstr + str 8/16/32) ────────────────────────────

    [Theory]
    [InlineData("a0", "")] // fixstr, length 0
    [InlineData("a161", "a")] // fixstr
    [InlineData("a3666f6f", "foo")] // fixstr
    [InlineData("a568656c6c6f", "hello")] // fixstr
    [InlineData("a2c3bc", "\u00fc")] // fixstr, ü (2 UTF-8 bytes)
    [InlineData("a3e6b0b4", "\u6c34")] // fixstr, 水 (3 UTF-8 bytes)
    [InlineData("a4f0908591", "\U00010151")] // fixstr, 𐅑 (4 UTF-8 bytes)
    public void TextString(string hex, string expected)
    {
        AssertRoundTrip(hex, expected, StringProxy.Instance);
    }

    [Fact]
    public void TextString_MaxFixStr()
    {
        // 31 bytes is the largest fixstr (format byte 0xa0 | length).
        var s = new string('A', 31);
        var expectedHex = "BF" + Convert.ToHexString(System.Text.Encoding.UTF8.GetBytes(s));
        AssertRoundTrip(expectedHex, s, StringProxy.Instance);
    }

    [Fact]
    public void TextString_Str8()
    {
        // 32 bytes spills over into str 8 (format byte 0xd9, single length byte).
        var s = new string('A', 32);
        var expectedHex = "D920" + Convert.ToHexString(System.Text.Encoding.UTF8.GetBytes(s));
        AssertRoundTrip(expectedHex, s, StringProxy.Instance);
    }

    [Fact]
    public void TextString_Str16()
    {
        // 256 bytes spills over into str 16 (format byte 0xda, 2 length bytes).
        var s = new string('A', 256);
        var expectedHex = "DA0100" + Convert.ToHexString(System.Text.Encoding.UTF8.GetBytes(s));
        AssertRoundTrip(expectedHex, s, StringProxy.Instance);
    }

    // ── Byte strings (bin 8/16/32) ────────────────────────────────

    [Fact]
    public void ByteString_Empty()
    {
        AssertRoundTrip("c400", Array.Empty<byte>(), ByteArrayProxy.Instance);
    }

    [Fact]
    public void ByteString_Bin8()
    {
        AssertRoundTrip("c40401020304", new byte[] { 1, 2, 3, 4 }, ByteArrayProxy.Instance);
    }

    [Fact]
    public void ByteString_Bin16()
    {
        // 256 bytes spills over into bin 16 (format byte 0xc5, 2 length bytes).
        var data = new byte[256];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = (byte)i;
        }
        var expectedHex = "C50100" + Convert.ToHexString(data);
        AssertRoundTrip(expectedHex, data, ByteArrayProxy.Instance);
    }

    // ── Float 32 (0xca) ───────────────────────────────────────────

    [Theory]
    [InlineData("ca4048f5c3", 3.14f)]
    [InlineData("ca47c35000", 100000.0f)]
    [InlineData("ca7f7fffff", float.MaxValue)]
    public void Float32(string hex, float expected)
    {
        AssertRoundTrip(hex, expected, F32Proxy.Instance);
    }

    // ── Float 64 (0xcb) ───────────────────────────────────────────

    [Theory]
    [InlineData("cb40091eb851eb851f", 3.14)]
    [InlineData("cb3ff199999999999a", 1.1)]
    [InlineData("cb7e37e43c8800759c", 1e300)]
    public void Float64(string hex, double expected)
    {
        AssertRoundTrip(hex, expected, F64Proxy.Instance);
    }

    [Fact]
    public void Float_SpecialValues()
    {
        AssertRoundTrip("ca7f800000", float.PositiveInfinity, F32Proxy.Instance);
        AssertRoundTrip("caff800000", float.NegativeInfinity, F32Proxy.Instance);
        AssertRoundTrip("cb7ff0000000000000", double.PositiveInfinity, F64Proxy.Instance);
        AssertRoundTrip("cbfff0000000000000", double.NegativeInfinity, F64Proxy.Instance);
    }

    // ── Arrays (fixarray + array 16) ──────────────────────────────

    [Fact]
    public void Array_Empty()
    {
        AssertCollectionRoundTrip<
            int[],
            ArrayProxy.Ser<int, I32Proxy>,
            ArrayProxy.De<int, I32Proxy>
        >("90", Array.Empty<int>());
    }

    [Fact]
    public void Array_FixArray()
    {
        AssertCollectionRoundTrip<
            int[],
            ArrayProxy.Ser<int, I32Proxy>,
            ArrayProxy.De<int, I32Proxy>
        >("93010203", new[] { 1, 2, 3 });
    }

    [Fact]
    public void Array_MaxFixArray()
    {
        // 15 elements is the largest fixarray (format byte 0x90 | length).
        var arr = Enumerable.Range(1, 15).ToArray();
        var expectedHex = "9F" + Convert.ToHexString(arr.Select(i => (byte)i).ToArray());
        AssertCollectionRoundTrip<
            int[],
            ArrayProxy.Ser<int, I32Proxy>,
            ArrayProxy.De<int, I32Proxy>
        >(expectedHex, arr);
    }

    [Fact]
    public void Array_Array16()
    {
        // 16 elements spills over into array 16 (format byte 0xdc, 2 length bytes).
        var arr = Enumerable.Range(1, 16).ToArray();
        var expectedHex = "DC0010" + Convert.ToHexString(arr.Select(i => (byte)i).ToArray());
        AssertCollectionRoundTrip<
            int[],
            ArrayProxy.Ser<int, I32Proxy>,
            ArrayProxy.De<int, I32Proxy>
        >(expectedHex, arr);
    }

    // ── Maps (fixmap + map 16) ────────────────────────────────────

    [Fact]
    public void Map_Empty()
    {
        AssertCollectionRoundTrip<
            Dictionary<int, int>,
            DictProxy.Ser<int, int, I32Proxy, I32Proxy>,
            DictProxy.De<int, int, I32Proxy, I32Proxy>
        >("80", new Dictionary<int, int>());
    }

    [Fact]
    public void Map_IntToInt()
    {
        // {1: 2, 3: 4}
        var hex = "8201020304";
        var bytes = Convert.FromHexString(hex);
        var actual = MsgPackSerializer.Deserialize<
            Dictionary<int, int>,
            IDeserialize<Dictionary<int, int>>
        >(bytes, DictProxy.De<int, int, I32Proxy, I32Proxy>.Instance);
        Assert.Equal(2, actual.Count);
        Assert.Equal(2, actual[1]);
        Assert.Equal(4, actual[3]);

        var serialized = MsgPackSerializer.Serialize(
            new Dictionary<int, int> { [1] = 2, [3] = 4 },
            DictProxy.Ser<int, int, I32Proxy, I32Proxy>.Instance
        );
        Assert.Equal(hex, Convert.ToHexString(serialized), StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void Map_StringToString()
    {
        // {"a": "A"}
        var hex = "81a161a141";
        AssertCollectionRoundTrip<
            Dictionary<string, string>,
            DictProxy.Ser<string, string, StringProxy, StringProxy>,
            DictProxy.De<string, string, StringProxy, StringProxy>
        >(hex, new Dictionary<string, string> { ["a"] = "A" });
    }

    // ── Custom types (encoded as a map keyed by member name) ──────

    [GenerateSerde]
    [SerdeTypeOptions(MemberFormat = MemberFormat.None)]
    private partial record Point
    {
        public int X { get; init; }
        public int Y { get; init; }
    }

    [Fact]
    public void Record_AsMap()
    {
        // {"X": 1, "Y": 2} -> fixmap(2) "X" 1 "Y" 2
        var hex = "82a15801a15902";
        var value = new Point { X = 1, Y = 2 };

        var serialized = MsgPackSerializer.Serialize(value);
        Assert.Equal(hex, Convert.ToHexString(serialized), StringComparer.OrdinalIgnoreCase);

        var bytes = Convert.FromHexString(hex);
        var actual = MsgPackSerializer.Deserialize<Point>(bytes);
        Assert.Equal(value, actual);
    }

    // ── Helpers ───────────────────────────────────────────────────

    private static void AssertRoundTrip<T, TProxy>(string hex, T expected, TProxy proxy)
        where TProxy : ISerialize<T>, IDeserializeProvider<T>
    {
        var bytes = Convert.FromHexString(hex);
        var actual = MsgPackSerializer.Deserialize<T, IDeserialize<T>>(bytes, TProxy.Instance);
        Assert.Equal(expected, actual);

        var serialized = MsgPackSerializer.Serialize(expected, proxy);
        Assert.Equal(hex, Convert.ToHexString(serialized), StringComparer.OrdinalIgnoreCase);
    }

    private static void AssertCollectionRoundTrip<T, TSer, TDe>(string hex, T expected)
        where TSer : ISerializeProvider<T>
        where TDe : IDeserializeProvider<T>
    {
        var bytes = Convert.FromHexString(hex);
        var actual = MsgPackSerializer.Deserialize<T, IDeserialize<T>>(bytes, TDe.Instance);
        Assert.Equal(expected, actual);

        var serialized = MsgPackSerializer.Serialize(expected, TSer.Instance);
        Assert.Equal(hex, Convert.ToHexString(serialized), StringComparer.OrdinalIgnoreCase);
    }
}
