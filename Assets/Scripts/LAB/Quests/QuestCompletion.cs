using UnityEngine;

namespace Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] private Quest quest;
        [SerializeField] private string goal;

        public void CompleteGoal()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var questList = player.GetComponent<QuestManager>();
            
            questList.CompleteGoal(quest, goal);
        }
    }
}
