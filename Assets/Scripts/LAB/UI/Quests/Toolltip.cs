using System.Linq;
using Quests;
using TMPro;
using UnityEngine;

namespace UI.Quests
{
    public class Toolltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Transform goalsParent;
        [SerializeField] private GameObject goalPrefab;
        [SerializeField] private GameObject goalIncompletePrefab;
        [SerializeField] private TextMeshProUGUI rewards;

        public void SetTooltipUI(QuestStatus questStatus)
        {
            title.text = questStatus.Quest.Name;
            foreach (var goal in questStatus.Quest.Goals)
            {
                var prefab = questStatus.IsGoalComplete(goal.id) ? goalPrefab : goalIncompletePrefab;
                var goalInstance = Instantiate(prefab, goalsParent);
                var goalText = goalInstance.GetComponentInChildren<TextMeshProUGUI>();
                
                if (goalText == null) continue;

                goalText.text = goal.description;
            }

            foreach (var reward in questStatus.Quest.Rewards)
            {
                rewards.text += reward.number + " " + reward.item.name + ".\n";
            }

            if (questStatus.Quest.Rewards.Any()) return;

            rewards.text = "Aucune récompense.";
        }
    }
}
