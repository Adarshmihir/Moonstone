using System.Collections.Generic;
using Quests;
using UnityEngine;

namespace UI.Quests
{
    public class QuestList : MonoBehaviour
    {
        [SerializeField] private List<Quest> questList;
        [SerializeField] private QuestItem questPrefab;
        
        // Start is called before the first frame update
        private void Start()
        {
            foreach (var quest in questList)
            {
                var instance = Instantiate(questPrefab, transform);
                var questItem = instance.GetComponent<QuestItem>();

                if (questItem == null) return;
                
                questItem.UpdateUI(quest);
            }
        }
    }
}
