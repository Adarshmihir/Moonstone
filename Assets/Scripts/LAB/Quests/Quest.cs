using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Moonstone/New Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] private List<string> goals;

        public string Name => name;
        public int Count => goals.Count;
        public IEnumerable<string> Goals => goals;
    }
}
