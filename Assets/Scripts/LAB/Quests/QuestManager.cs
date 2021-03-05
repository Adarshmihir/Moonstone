using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Quests
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private List<QuestStatus> questStatus = new List<QuestStatus>();

        public IEnumerable<QuestStatus> QuestStatuses => questStatus;

        public event Action OnUpdate;

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) return;
            
            questStatus.Add(new QuestStatus(quest));
            OnUpdate?.Invoke();
        }

        private bool HasQuest(Object quest)
        {
            return questStatus.Any(status => status.Quest == quest);
        }

        public void CompleteGoal(Quest quest, string goal)
        {
            var status = questStatus.FirstOrDefault(element => element.Quest == quest);
            
            status?.CompleteGoal(goal);
            OnUpdate?.Invoke();
        }
    }
}
