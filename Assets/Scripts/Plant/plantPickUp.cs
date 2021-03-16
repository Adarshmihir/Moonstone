using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quests;

public class plantPickUp : Interactable
{
    public Plant plant;

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    private void PickUp()
    {
        Debug.Log("Picking up " + plant.name + " !");
        GameObject player = GameObject.Find("Player");

        var questList = player.GetComponent<QuestManager>();

        var evaluatedQuest = questList.Evaluate("HasQuest", "PlantQuest");

        if (evaluatedQuest != null)
        {
            questList.CompleteGoal(questList.GetQuestByName("PlantQuest"), "0");
        }

        /*switch (plant.name)
        {
            case ("Plant1"):
            {
                    player.GetComponent<PlantCounter>().addPlant1();
                    if (evaluatedQuest != null)
                    {
                        questList.CompleteGoal(questList.GetQuestByName("PlantQuest"), "0");
                    }
                    break;
            }
            case ("Plant2"):
            {
                    player.GetComponent<PlantCounter>().addPlant2();
                    if (evaluatedQuest != null)
                    {
                        questList.CompleteGoal(questList.GetQuestByName("PlantQuest"), "2");
                    }
                    break;
            }
            case ("Plant3"):
            {
                    player.GetComponent<PlantCounter>().addPlant3();
                    break;
            }
        }*/

        //GameManager.Instance.uiManager.CanvasRessource.GetComponent<FillPlantsBar>().PickPlant(0.25f);
        GameObject ressourceCanvas = GameObject.Find("CanvasRessource");
        ressourceCanvas.GetComponent<FillPlantsBar>().PickPlant(0.25f);
       
        player.GetComponent<PlantCounter>().showCount();
        Destroy(transform.gameObject);
    }
}
