using Quests;
using TMPro;
using UnityEngine;

namespace UI.Quests
{
    public class QuestItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI progress;
        
        public Quest Quest { get; private set; }

        public void UpdateUI(Quest quest)
        {
            Quest = quest;
            
            title.text = quest.Name;
            progress.text = "0/" + quest.Count;
        }
    }
}
