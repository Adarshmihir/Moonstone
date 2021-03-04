using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private List<QuestStatus> questStatus = new List<QuestStatus>();

        public IEnumerable<QuestStatus> QuestStatuses => questStatus;

        public event Action onUpdate;

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) return;
            
            questStatus.Add(new QuestStatus(quest));

            if (onUpdate == null) return;
            
            onUpdate();
        }

        private bool HasQuest(Quest quest)
        {
            foreach (var status in questStatus)
            {
                if (status.Quest == quest)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
