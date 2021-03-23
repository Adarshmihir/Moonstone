using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] private List<CompletionData> completionData = new List<CompletionData>();

        public void CompleteGoal(int index)
        {
            if (index >= completionData.Count) return;
            
            var player = GameObject.FindGameObjectWithTag("Player");
            var questList = player.GetComponent<QuestManager>();
            
            questList.CompleteGoal(completionData[index].Quest, completionData[index].Goal);
        }

        public void CompleteQuest(int index)
        {
            if (index >= completionData.Count) return;
            
            var player = GameObject.FindGameObjectWithTag("Player");
            var questList = player.GetComponent<QuestManager>();
            
            questList.CompleteQuest(completionData[index].Quest);

            player.GetComponent<PlayerFX>().PlayQuestCompletion();
        }

        [Serializable]
        private class CompletionData
        {
            [SerializeField] private Quest quest;
            [SerializeField] private string goal;

            public Quest Quest => quest;
            public string Goal => goal;
        }
    }
}
