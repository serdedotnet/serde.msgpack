// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.



namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class Error : IGenericEquality<Error>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? error_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public string error_name { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public string description { get; set; }

        public bool Equals(Error obj)
        {
            return
                this.error_id.TrueEquals(obj.error_id) &&
                this.error_name.TrueEqualsString(obj.error_name) &&
                this.description.TrueEqualsString(obj.description);
        }
    }
}
