using IdleGuildDemo.Runtime;

namespace IdleGuildDemo.Systems
{
    /// <summary>
    /// Owns future linear quest-chain progress and reward claim rules.
    /// </summary>
    public sealed class QuestSystem
    {
        public string GetCurrentQuest(SaveData saveData)
        {
            // TODO: Return the active quest id after quest flow is implemented.
            return saveData != null && saveData.profile != null ? saveData.profile.currentQuestId : string.Empty;
        }
    }
}
