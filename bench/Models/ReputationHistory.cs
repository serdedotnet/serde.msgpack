// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System;

namespace Benchmark.Models
{
    [MessagePack.MessagePackObject]
    [Serde.GenerateSerde]
    public partial class ReputationHistory : IGenericEquality<ReputationHistory>
    {
        [Serde.GenerateSerde(AsUnderlying = true)]
        public enum ReputationHistoryType : byte
        {
            asker_accepts_answer = 1,
            asker_unaccept_answer = 2,
            answer_accepted = 3,
            answer_unaccepted = 4,

            voter_downvotes = 5,
            voter_undownvotes = 6,
            post_downvoted = 7,
            post_undownvoted = 8,

            post_upvoted = 9,
            post_unupvoted = 10,

            suggested_edit_approval_received = 11,

            post_flagged_as_spam = 12,
            post_flagged_as_offensive = 13,

            bounty_given = 14,
            bounty_earned = 15,
            bounty_cancelled = 16,

            post_deleted = 17,
            post_undeleted = 18,

            association_bonus = 19,
            arbitrary_reputation_change = 20,

            vote_fraud_reversal = 21,

            post_migrated = 22,

            user_deleted = 23,
        }

        [MessagePack.Key(0)]
        [Serde.SerdeMemberOptions(Ordinal = 0)]
        public int? user_id { get; set; }

        [MessagePack.Key(1)]
        [Serde.SerdeMemberOptions(Ordinal = 1)]
        public DateTime? creation_date { get; set; }

        [MessagePack.Key(2)]
        [Serde.SerdeMemberOptions(Ordinal = 2)]
        public int? post_id { get; set; }

        [MessagePack.Key(3)]
        [Serde.SerdeMemberOptions(Ordinal = 3)]
        public int? reputation_change { get; set; }

        [MessagePack.Key(4)]
        [Serde.SerdeMemberOptions(Ordinal = 4)]
        public ReputationHistoryType? reputation_history_type { get; set; }

        public bool Equals(ReputationHistory obj)
        {
            return
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.post_id.TrueEquals(obj.post_id) &&
                this.reputation_change.TrueEquals(obj.reputation_change) &&
                this.reputation_history_type.TrueEquals(obj.reputation_history_type) &&
                this.user_id.TrueEquals(obj.user_id);
        }
    }
}
