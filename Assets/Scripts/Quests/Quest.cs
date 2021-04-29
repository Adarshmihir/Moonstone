using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Moonstone/New Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] private List<Goal> goals;
        [SerializeField] private List<Reward> rewards;

        [System.Serializable]
        public class Reward
        {
            public int number;
            public Item2 item;
        }
        
        [System.Serializable]
        public class Goal
        {
            public string id;
            public string description;
        }

        public string Name => name;
        public int Count => goals.Count;
        public IEnumerable<Goal> Goals => goals;
        public IEnumerable<Reward> Rewards => rewards;

        public bool HasGoal(string id)
        {
            return goals.Any(goal => goal.id == id);
        }
    }
}
