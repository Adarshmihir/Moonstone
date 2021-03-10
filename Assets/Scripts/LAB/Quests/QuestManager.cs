using System;
using System.Collections.Generic;
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
            OnUpdate?.Invoke();
        }

        public void CompleteQuest(Quest quest)
        {
            var status = questStatus.FirstOrDefault(element => element.Quest == quest);
            
            if (status != null && status.IsQuestComplete())
            {
                GiveRewards(quest);
                status.EndQuest();
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

        public bool? Evaluate(string evaluateName, string parameter)
        {
            if (parameter.Length == 0) return null;
            
            var evaluatedQuest = GetQuestByName(parameter);
            if (evaluatedQuest == null) return false;
            
            switch (evaluateName)
            {
                case "HasQuest":
                    return evaluatedQuest != null;
                case "HasCompleted":
                    return questStatus.FirstOrDefault(status => status.Quest == evaluatedQuest)?.IsQuestComplete();
                case "HasDone":
                    return questStatus.FirstOrDefault(status => status.Quest == evaluatedQuest)?.Done;
                // TODO : Add evaluation ?
            }
            return null;
        }
    }
}
