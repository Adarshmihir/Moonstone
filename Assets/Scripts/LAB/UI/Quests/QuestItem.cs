using Quests;
using TMPro;
using UnityEngine;

namespace UI.Quests
{
    public class QuestItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI progress;
        
        public QuestStatus QuestStatus { get; private set; }

        public void UpdateUI(QuestStatus questStatus)
        {
            QuestStatus = questStatus;
            
            title.text = QuestStatus.Quest.Name;
            progress.text = QuestStatus.Status.Count + "/" + QuestStatus.Quest.Count;
        }
    }
}
