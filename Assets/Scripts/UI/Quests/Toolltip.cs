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

        public void SetQuestTooltipUI(QuestStatus questStatus)
        {
            foreach (Transform goal in goalsParent)
            {
                Destroy(goal.gameObject);
            }
            
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
                rewards.text += reward.number + " " + reward.item.Name + ".\n";
            }

            if (questStatus.Quest.Rewards.Any()) return;

            rewards.text = "Aucune récompense.";
        }

        public void SetItemTooltipUI(ItemObject item, InventoryObject inventoryObject, int index)
        {
            foreach (Transform goal in goalsParent)
            {
                Destroy(goal.gameObject);
            }
            var itemData = inventoryObject.GetSlots[index].item.buffs.Length > 0 ? 
                inventoryObject.GetSlots[index].item
                : item.data;
            title.text = itemData.Name;
            foreach (var modifier in itemData.buffs)
            {
                var modInstance = Instantiate(goalPrefab, goalsParent);
                var modText = modInstance.GetComponentInChildren<TextMeshProUGUI>();
                
                if (modText == null) continue;

                if (modifier.value > 0)
                {
                    modText.text = "+" + modifier.value + " en " + modifier.attribute.ToString().ToLower();
                }
                else
                {
                    modText.text = "+ [" + modifier.min + "-" + modifier.max + "] en " + modifier.attribute.ToString().ToLower();
                }
            }
            
            if (item.Spell == null) return;
            
            rewards.text = item.Spell.name;
        }
    }
}
