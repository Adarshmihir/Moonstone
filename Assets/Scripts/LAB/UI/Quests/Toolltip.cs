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
        [SerializeField] private TextMeshProUGUI rewards;

        public void SetTooltipUI(Quest quest)
        {
            title.text = quest.Name;
            foreach (var goal in quest.Goals)
            {
                var goalInstance = Instantiate(goalPrefab, goalsParent);
                var goalText = goalInstance.GetComponentInChildren<TextMeshProUGUI>();
                
                if (goalText == null) continue;

                goalText.text = goal;
            }
            rewards.text = "TODO";
        }
    }
}
