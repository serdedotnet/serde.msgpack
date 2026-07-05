
using CsCheck;
using MessagePack;

namespace Serde.MsgPack.Tests;

/// <summary>
/// Property-based tests (via CsCheck) that exercise the serializer over a wide
/// range of randomly generated values. Two properties are checked:
///
/// 1. Oracle: for every value, <see cref="MsgPackSerializer"/> produces exactly
///    the same bytes as the reference MessagePack-CSharp library.
/// 2. Round-trip: every value survives a serialize/deserialize cycle unchanged.
/// </summary>
public class MsgPackFsCheck
{
    private const int Iter = 1000;

    // Strings built from valid Unicode scalar values (no unpaired surrogates),
    // covering the full multi-byte UTF-8 range including astral planes.
    private static readonly Gen<string> GenString =
        Gen.Int[0, 0x10FFFF]
            .Where(cp => cp < 0xD800 || cp > 0xDFFF)
            .Select(char.ConvertFromUtf32)
            .Array
            .Select(parts => string.Concat(parts));

    private static readonly Gen<byte[]> GenBytes = Gen.Byte.Array[0, 600];

    private static readonly Gen<int[]> GenIntArray = Gen.Int.Array[0, 600];

    // ── Oracle: output must match the MessagePack reference library ──

    [Fact]
    public void OracleBool() => AssertOracle(Gen.Bool, BoolProxy.Instance);

    [Fact]
    public void OracleSByte() => AssertOracle(Gen.SByte, I8Proxy.Instance);

    [Fact]
    public void OracleByte() => AssertOracle(Gen.Byte, U8Proxy.Instance);

    [Fact]
    public void OracleShort() => AssertOracle(Gen.Short, I16Proxy.Instance);

    [Fact]
    public void OracleUShort() => AssertOracle(Gen.UShort, U16Proxy.Instance);

    [Fact]
    public void OracleInt() => AssertOracle(Gen.Int, I32Proxy.Instance);

    [Fact]
    public void OracleUInt() => AssertOracle(Gen.UInt, U32Proxy.Instance);

    [Fact]
    public void OracleLong() => AssertOracle(Gen.Long, I64Proxy.Instance);

    [Fact]
    public void OracleULong() => AssertOracle(Gen.ULong, U64Proxy.Instance);

    [Fact]
    public void OracleFloat() => AssertOracle(Gen.Float, F32Proxy.Instance);

    [Fact]
    public void OracleDouble() => AssertOracle(Gen.Double, F64Proxy.Instance);

    [Fact]
    public void OracleString() => AssertOracle(GenString, StringProxy.Instance);

    [Fact]
    public void OracleBytes() => AssertOracle(GenBytes, ByteArrayProxy.Instance);

    [Fact]
    public void OracleIntArray() =>
        AssertCollectionOracle<int[], ArrayProxy.Ser<int, I32Proxy>>(GenIntArray);

    // ── Round-trip: serialize then deserialize yields the original value ──

    [Fact]
    public void RoundTripBool() => AssertRoundTrip(Gen.Bool, BoolProxy.Instance);

    [Fact]
    public void RoundTripSByte() => AssertRoundTrip(Gen.SByte, I8Proxy.Instance);

    [Fact]
    public void RoundTripByte() => AssertRoundTrip(Gen.Byte, U8Proxy.Instance);

    [Fact]
    public void RoundTripShort() => AssertRoundTrip(Gen.Short, I16Proxy.Instance);

    [Fact]
    public void RoundTripUShort() => AssertRoundTrip(Gen.UShort, U16Proxy.Instance);

    [Fact]
    public void RoundTripInt() => AssertRoundTrip(Gen.Int, I32Proxy.Instance);

    [Fact]
    public void RoundTripUInt() => AssertRoundTrip(Gen.UInt, U32Proxy.Instance);

    [Fact]
    public void RoundTripLong() => AssertRoundTrip(Gen.Long, I64Proxy.Instance);

    [Fact]
    public void RoundTripULong() => AssertRoundTrip(Gen.ULong, U64Proxy.Instance);

    [Fact]
    public void RoundTripFloat() => AssertRoundTrip(Gen.Float, F32Proxy.Instance);

    [Fact]
    public void RoundTripDouble() => AssertRoundTrip(Gen.Double, F64Proxy.Instance);

    [Fact]
    public void RoundTripString() => AssertRoundTrip(GenString, StringProxy.Instance);

    [Fact]
    public void RoundTripBytes() => AssertRoundTrip(GenBytes, ByteArrayProxy.Instance);

    [Fact]
    public void RoundTripIntArray() =>
        AssertCollectionRoundTrip<int[], ArrayProxy.Ser<int, I32Proxy>, ArrayProxy.De<int, I32Proxy>>(GenIntArray);

    // ── Helpers ───────────────────────────────────────────────────

    private static void AssertOracle<T, TProxy>(Gen<T> gen, TProxy proxy)
        where TProxy : ISerialize<T>
    {
        gen.Sample(value =>
        {
            byte[] expected = MessagePackSerializer.Serialize(value);
            byte[] actual = MsgPackSerializer.Serialize(value, proxy);
            Assert.Equal(expected, actual);
        }, iter: Iter);
    }

    private static void AssertCollectionOracle<T, TSer>(Gen<T> gen)
        where TSer : ISerializeProvider<T>
    {
        gen.Sample(value =>
        {
            byte[] expected = MessagePackSerializer.Serialize(value);
            byte[] actual = MsgPackSerializer.Serialize(value, TSer.Instance);
            Assert.Equal(expected, actual);
        }, iter: Iter);
    }

    private static void AssertRoundTrip<T, TProxy>(Gen<T> gen, TProxy proxy)
        where TProxy : ISerialize<T>, IDeserializeProvider<T>
    {
        gen.Sample(value =>
        {
            byte[] bytes = MsgPackSerializer.Serialize(value, proxy);
            T actual = MsgPackSerializer.Deserialize<T, IDeserialize<T>>(bytes, TProxy.Instance);
            Assert.Equal(value, actual);
        }, iter: Iter);
    }

    private static void AssertCollectionRoundTrip<T, TSer, TDe>(Gen<T> gen)
        where TSer : ISerializeProvider<T>
        where TDe : IDeserializeProvider<T>
    {
        gen.Sample(value =>
        {
            byte[] bytes = MsgPackSerializer.Serialize(value, TSer.Instance);
            T actual = MsgPackSerializer.Deserialize<T, IDeserialize<T>>(bytes, TDe.Instance);
            Assert.Equal(value, actual);
        }, iter: Iter);
    }
}
