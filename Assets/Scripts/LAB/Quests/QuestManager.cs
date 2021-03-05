using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Quests
{
    public class QuestManager : MonoBehaviour, IEvaluator
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
        
        private Quest GetQuestByName(string questName)
        {
            return questStatus.FirstOrDefault(status => status.Quest.Name == questName)?.Quest;
        }

        public void CompleteGoal(Quest quest, string goal)
        {
            var status = questStatus.FirstOrDefault(element => element.Quest == quest);
            
            status?.CompleteGoal(goal);
            if (status != null && status.IsQuestDone())
            {
                GiveRewards(quest);
            }
            OnUpdate?.Invoke();
        }

        private static void GiveRewards(Quest quest)
        {
            foreach (var reward in quest.Rewards)
            {
                // TODO : Add items to inventory
            }
        }

        public bool? Evaluate(string evaluateName, string[] parameters)
        {
            if (parameters.Length == 0) return null;
            
            var evaluatedQuest = GetQuestByName(parameters[0]);
            switch (evaluateName)
            {
                case "HasQuest":
                    return evaluatedQuest != null;
                case "CompletedQuest":
                    return questStatus.FirstOrDefault(status => status.Quest == evaluatedQuest)?.IsQuestDone();
                // TODO : Add evaluation ?
            }
            return null;
        }
    }
}
