using Quests;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private Quest quest;

    public void GiveQuest()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var questList = player.GetComponent<QuestManager>();
        
        questList.AddQuest(quest);
    }
}
