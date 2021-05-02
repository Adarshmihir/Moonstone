using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quests;

public class CheckGold : MonoBehaviour
{
    public int goldNeeded;
    public Quest quest;
    public string idGoal;
    private GameObject gm;

    private void Start()
    {
        gm = GameObject.Find("GameManager");
    }
    
    // Update is called once per frame
    void Update()
    {
        var player = GameManager.Instance.player;
        var questList = player.GetComponent<QuestManager>();

        var evaluatedQuest = questList.Evaluate("HasQuest", quest.Name);
        var questStatus = questList.GetQuestStatusByName(quest.Name);

        if (evaluatedQuest != null && GameManager.Instance.player.inventory.gold >= goldNeeded && questStatus!=null && !questStatus.Status.Contains(idGoal))
        {
            questList.CompleteGoal(questStatus.Quest, idGoal);
        }
    }
}
