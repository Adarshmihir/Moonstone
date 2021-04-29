using System.Collections.Generic;
using Quests;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private List<Quest> quests;

    public void GiveQuest(int index)
    {
        if (index >= quests.Count) return;
        
        var player = GameObject.FindGameObjectWithTag("Player");
        var questList = player.GetComponent<QuestManager>();
        
        questList.AddQuest(quests[index]);
    }
}
