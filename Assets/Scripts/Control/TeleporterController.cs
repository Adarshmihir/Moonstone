using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quests;

public class TeleporterController : MonoBehaviour
{
    private GameObject player;
    public Quest quest;
    public GameObject teleporter;

    public int statusQuest = 1; //1 = hasQuest, 2 = QuestCompleted, 3 = QuestDone
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.player.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var play = GameManager.Instance.player;
        var evaluatedQuest = play.GetComponent<QuestManager>().Evaluate("HasDone", quest.Name);

        if (statusQuest == 1 && play.GetComponent<QuestManager>().HasQuest(quest))
        {
            teleporter.SetActive(true);
        }
        else if (statusQuest == 2 && evaluatedQuest == true)
        {
            teleporter.SetActive(true);
        }
        else
        {
            teleporter.SetActive(false);
        }
    }
}
