using System;

namespace IdleGuildDemo.Runtime
{
    [Serializable]
    public sealed class TalentState
    {
        public string talentId = string.Empty;
        public int rank;

        public TalentState()
        {
        }

        public TalentState(string talentId, int rank)
        {
            this.talentId = talentId ?? string.Empty;
            this.rank = Math.Max(0, rank);
        }

        public void Normalize()
        {
            if (talentId == null)
            {
                talentId = string.Empty;
            }

            if (rank < 0)
            {
                rank = 0;
            }
        }
    }
}
