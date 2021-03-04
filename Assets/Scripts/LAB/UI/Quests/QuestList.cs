using Quests;
using UnityEngine;

namespace UI.Quests
{
    public class QuestList : MonoBehaviour
    {
        [SerializeField] private QuestItem questPrefab;

        private QuestManager _questManager;
        
        // Start is called before the first frame update
        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _questManager = player.GetComponent<QuestManager>();

            _questManager.onUpdate += RefreshList;
            RefreshList();
        }

        private void RefreshList()
        {
            foreach (var quest in _questManager.QuestStatuses)
            {
                var instance = Instantiate(questPrefab, transform);
                var questItem = instance.GetComponent<QuestItem>();

                if (questItem == null) return;
                
                questItem.UpdateUI(quest);
            }
        }
    }
}
