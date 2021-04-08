using System;
using Combat;
using Core;
using Movement;
using UnityEngine;
using Quests;

public class PlantPickUp : MonoBehaviour, IAction
{
    [SerializeField] private float healthAmount = .25f;
    [SerializeField] private float timer = 5f;

    private Fighter _fighter;
    private Mover _mover;
    private bool _isTakingPlant;

    private void Start()
    {
        _mover = GameManager.Instance.player.GetComponent<Mover>();
        _fighter = GameManager.Instance.player.GetComponent<Fighter>();
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
        if (GameManager.Instance.uiManager.CanvasRessourceGO.GetComponent<FillPlantsBar>().currentHeal >= 1f)
        {
            GameManager.Instance.FeedbackMessage.SetMessage("Barre de soin déjà pleine");
            return;
        }
        
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
        
        GameManager.Instance.uiManager.CanvasRessourceGO.GetComponent<FillPlantsBar>().PickPlant(healthAmount);

        gameObject.SetActive(false);
        Invoke(nameof(UpdatePlantVisibility), timer);
        
        Cancel();
    }

    private void UpdatePlantVisibility()
    {
        gameObject.SetActive(true);
    }

    public void Cancel()
    {
        _isTakingPlant = false;
    }
}
