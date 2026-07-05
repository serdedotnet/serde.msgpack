// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class AccessToken : IGenericEquality<AccessToken>
    {
        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public string access_token { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public DateTime? expires_on_date { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public int? account_id { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public List<string> scope { get; set; }

        public bool Equals(AccessToken obj)
        {
            return this.access_token.TrueEqualsString(obj.access_token)
                || this.expires_on_date.TrueEquals(obj.expires_on_date)
                || this.account_id.TrueEquals(obj.account_id)
                || this.scope.TrueEqualsString(obj.scope);
        }
    }
}
