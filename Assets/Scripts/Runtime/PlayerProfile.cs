using System;

namespace IdleGuildDemo.Runtime
{
    [Serializable]
    public sealed class PlayerProfile
    {
        public string playerId = "local_player";
        public string displayName = "Reviewer";
        public int saveVersion = 1;
    }
}
