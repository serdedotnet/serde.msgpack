// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BenchmarkDotNet.Attributes;
using MessagePack;
using Serde;

namespace Benchmarks
{
    [GenericTypeArguments(typeof(Location), typeof(LocationWrap))]
    public class DeserializeFromString<T, U>
        where T : Serde.IDeserializeProvider<T>
        where U : Serde.IDeserializeProvider<T>
    {
        private byte[] value = null!;

        private readonly IDeserialize<T> _proxy = T.DeserializeInstance;
        private readonly IDeserialize<T> _manualProxy = U.DeserializeInstance;

        [GlobalSetup]
        public void Setup()
        {
            value = DataGenerator.GenerateDeserialize<T>();
        }

        [Benchmark]
        public T? MessagePack()
        {
            return MessagePackSerializer.Deserialize<T>(value);
        }

        [Benchmark]
        public T SerdeMsgPack() => Serde.MsgPack.MsgPackSerializer.Deserialize<T, IDeserialize<T>>(value, _proxy);

        [Benchmark]
        public T SerdeMsgPackManual() => Serde.MsgPack.MsgPackSerializer.Deserialize<T, IDeserialize<T>>(value, _manualProxy);

        // DataContractJsonSerializer does not provide an API to serialize to string
        // so it's not included here (apples vs apples thing)
    }
}