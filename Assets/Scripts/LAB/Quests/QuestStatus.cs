﻿using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    [System.Serializable]
    public class QuestStatus
    {
        [SerializeField] private Quest quest;
        [SerializeField] private List<string> status = new List<string>();

        public Quest Quest => quest;
        public List<string> Status => status;

        public QuestStatus(Quest newQuest)
        {
            quest = newQuest;
        }

        public bool IsGoalComplete(string goal)
        {
            return status.Contains(goal);
        }
    }
}
