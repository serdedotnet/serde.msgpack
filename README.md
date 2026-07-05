# Serde.MsgPack

A [MessagePack](https://msgpack.org/) ([spec](https://github.com/msgpack/msgpack/blob/master/spec.md)) serializer and deserializer for .NET, built on the [Serde](https://github.com/serdedotnet/serde) serialization framework.

MessagePack is a compact, efficient binary serialization format. It is well suited to network protocols, caching, and anywhere a small binary encoding is needed.

## Installation

```sh
dotnet add package Serde.MsgPack
```

## Usage

### Serialize and deserialize simple types

Built-in types are serialized through their Serde proxy:

```csharp
using Serde;
using Serde.MsgPack;

byte[] bytes = MsgPackSerializer.Serialize(42, I32Proxy.Instance);
int value = MsgPackSerializer.Deserialize<int, IDeserialize<int>>(bytes, I32Proxy.Instance);
```

### Serialize and deserialize custom types

Use the `[GenerateSerde]` attribute from the Serde framework to generate serialization code for your types:

```csharp
using Serde;
using Serde.MsgPack;

[GenerateSerde]
public partial record Person
{
    public required string Name { get; init; }
    public int Age { get; init; }
}

var person = new Person { Name = "Alice", Age = 30 };
byte[] bytes = MsgPackSerializer.Serialize(person);
Person deserialized = MsgPackSerializer.Deserialize<Person>(bytes);
```

Custom types are encoded as a MessagePack map keyed by member name.

### Collections

Arrays, lists, and dictionaries are supported through their Serde proxies:

```csharp
using Serde;
using Serde.MsgPack;

byte[] bytes = MsgPackSerializer.Serialize(new[] { 1, 2, 3 }, ArrayProxy.Ser<int, I32Proxy>.Instance);
int[] values = MsgPackSerializer.Deserialize<int[], IDeserialize<int[]>>(
    bytes, ArrayProxy.De<int, I32Proxy>.Instance);
```

## Supported types

- Integers: `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `Int128`, `UInt128`
- Floating point: `float`, `double`
- `bool`, `char`, `string`
- `DateTime`, `DateTimeOffset` (encoded as ISO-8601 strings)
- Byte arrays (encoded with the `bin` family)
- Nullable reference types
- Arrays and dictionaries
- Custom types via `[GenerateSerde]`
- Enums

## Related projects

- [Serde](https://github.com/serdedotnet/serde) — the underlying serialization framework
- [Serde.Cbor](https://github.com/serdedotnet/cbor) — a CBOR backend built on the same framework

## License

BSD-3-Clause. See [LICENSE](LICENSE) for details.
