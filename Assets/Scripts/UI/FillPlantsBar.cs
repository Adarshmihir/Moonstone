using UnityEngine;
using UnityEngine.UI;
using Quests;

public class FillPlantsBar : MonoBehaviour
{



    public Image healingBarFront; // frontend value of heal
    public float currentHeal; // computed value of heal
    public float healIncrease;
    public float healDecrease;

    public float maxLife = 1f;
    public float currentLife; // computed value of hp
    public float missingLife;
    public float lifeIncrease;

    public bool isHealing;
    public bool isPickingPlant;

    public Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        healthSlider = GameObject.FindObjectOfType<HealthGlobeControl>().healthSlider;
        healingBarFront = GameObject.FindGameObjectWithTag("HealFront").GetComponent<Image>();
    
        currentLife = healthSlider.value; // Compute current life
        currentHeal = healingBarFront.fillAmount; // Compute current heal
    }

    // Update is called once per frame
    void Update()
    {
        if (isHealing)
        {
            if (healingBarFront.fillAmount > currentHeal)
            {
                healingBarFront.fillAmount -= healDecrease * Time.deltaTime;
                healthSlider.value += lifeIncrease * Time.deltaTime;
            }
            else
            {
                healingBarFront.fillAmount = currentHeal;
                healthSlider.value = currentLife;
                isHealing = false;
            }
        }

        if (isPickingPlant)
        {
            if (healingBarFront.fillAmount < currentHeal)
            {
                healingBarFront.fillAmount += healIncrease * Time.deltaTime;
            }
            else
            {
                healingBarFront.fillAmount = currentHeal;
                isPickingPlant = false;
            }
        }

        if (healthSlider.value <= 0f)
            Debug.Log("You're dead !");
    }

    public void UsePlant()
    {

        var player = GameManager.Instance.player;
        var questList = player.GetComponent<QuestManager>();

        var evaluatedQuest = questList.Evaluate("HasQuest", "PlantQuest");
        if (evaluatedQuest != null)
        {
            questList.CompleteGoal(questList.GetQuestByName("PlantQuest"), "1");
        }

        player.GetComponent<PlayerFX>().PlayEatPlant();

        Debug.Log("Using plant !");
        missingLife = maxLife - healthSlider.value; // Compute missing life
        healDecrease = Mathf.Min(currentHeal, missingLife);
        lifeIncrease = Mathf.Min(currentHeal, missingLife);

        if (missingLife > 0f && currentHeal > 0f)
        { // If we need to regen
            currentHeal -= healDecrease;
            currentLife += lifeIncrease;
        }
        else
        {
            if (currentHeal <= 0f)
                Debug.Log("Not enough heal to regen your life !"); // No healing available
            else
                Debug.Log("You don't need to heal !");

            isHealing = false;
            return;
        }
        isHealing = true;
    }

    public void TakeDamage(float amount)
    {
        if (currentLife - amount >= 0f)
            currentLife -= amount;
        else
        {
            currentLife = 0f;
        }
        healthSlider.value = currentLife;
    }

    public void PickPlant(float healAmount)
    {
        if (currentHeal + healAmount < 1f)
        {
            healIncrease = healAmount;
            currentHeal += healIncrease;
        }
        else
        {
            healIncrease = 1f - currentHeal;
            currentHeal = 1f;
        }
        isPickingPlant = true;
    }
}
