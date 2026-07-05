// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class WritePermission : IGenericEquality<WritePermission>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? user_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public string object_type { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public bool? can_add { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public bool? can_edit { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public bool? can_delete { get; set; }

        [MessagePack.Key(5)]
        [Serde.SerdeMemberOptions(Ordinal = 5)]
        public int? max_daily_actions { get; set; }

        [MessagePack.Key(6)]
        [Serde.SerdeMemberOptions(Ordinal = 6)]
        public int? min_seconds_between_actions { get; set; }

        public bool Equals(WritePermission obj)
        {
            return this.can_add.TrueEquals(obj.can_add)
                && this.can_delete.TrueEquals(obj.can_delete)
                && this.can_edit.TrueEquals(obj.can_edit)
                && this.max_daily_actions.TrueEquals(obj.max_daily_actions)
                && this.min_seconds_between_actions.TrueEquals(obj.min_seconds_between_actions)
                && this.object_type.TrueEqualsString(obj.object_type)
                && this.user_id.TrueEquals(obj.user_id);
        }
    }
}
