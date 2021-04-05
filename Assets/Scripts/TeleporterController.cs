using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quests;

public class TeleporterController : MonoBehaviour
{
    private GameObject player;
    public Quest quest;
    public GameObject teleporter;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.player.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var play = GameManager.Instance.player;
        if(play.GetComponent<QuestManager>().HasQuest(quest))
        {
            teleporter.SetActive(true);
        }
        else
        {
            teleporter.SetActive(false);
        }
    }
}
