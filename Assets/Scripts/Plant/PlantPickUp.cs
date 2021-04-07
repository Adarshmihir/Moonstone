using Combat;
using Core;
using Movement;
using UnityEngine;
using Quests;

public class PlantPickUp : MonoBehaviour, IAction
{
    [SerializeField] private float healthAmount = .25f;

    private Fighter _fighter;
    private Mover _mover;
    private bool _isTakingPlant;
    
    public bool IsActive { get; private set; }

    private void Start()
    {
        _mover = GameManager.Instance.player.GetComponent<Mover>();
        _fighter = GameManager.Instance.player.GetComponent<Fighter>();

        IsActive = true;
    }

    private void Update()
    {
        if (!_isTakingPlant) return;
        
        if (!_fighter.GetIsInRange(transform.position, 1f))
        {
            // Move towards the plant until it is close enough
            _mover.MoveTo(transform.position);
        }
        else
        {
            // Cancel movement action and start picking up
            _mover.Cancel();
            PickUp();
        }
    }

    public void PickPlantBehaviour()
    {
        GetComponent<ActionScheduler>().StartAction(this);
        _isTakingPlant = true;
    }

    private void PickUp()
    {
        var questList = GameManager.Instance.player.GetComponent<QuestManager>();

        if (questList != null)
        {
            var evaluatedQuest = questList.Evaluate("HasQuest", "PlantQuest");

            if (evaluatedQuest != null)
            {
                questList.CompleteGoal(questList.GetQuestByName("PlantQuest"), "0");
            }
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
        
        GameManager.Instance.uiManager.CanvasRessourceGO.GetComponent<FillPlantsBar>().PickPlant(healthAmount);

        IsActive = false;
        
        Cancel();

        //var resourceCanvas = GameObject.Find("CanvasRessource");
        //resourceCanvas.GetComponent<FillPlantsBar>().PickPlant(0.25f);

        //Destroy(transform.gameObject);
    }

    public void Cancel()
    {
        _isTakingPlant = false;
    }
}
