// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class AccountMerge : IGenericEquality<AccountMerge>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? old_account_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public int? new_account_id { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public DateTime? merge_date { get; set; }

        public bool Equals(AccountMerge obj)
        {
            return this.old_account_id.TrueEquals(obj.old_account_id)
                && this.new_account_id.TrueEquals(obj.new_account_id)
                && this.merge_date.TrueEquals(obj.merge_date);
        }
    }
}
