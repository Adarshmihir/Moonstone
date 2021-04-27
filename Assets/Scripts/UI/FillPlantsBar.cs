using System;
using DuloGames.UI;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.UI;
using Quests;
using ResourcesHealth;

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

    // Tween controls
    [NonSerialized] private readonly TweenRunner<FloatTween> m_FloatTweenRunner;
    public Slider healthSlider;
    public UIProgressBar bar;
    public float Duration = 5f;
    public TweenEasing Easing = TweenEasing.InOutQuint;
    public Text m_Text;
    public Test_UIProgressBar.TextVariant m_TextVariant = Test_UIProgressBar.TextVariant.Percent;
    public int m_TextValue = 100;
    public string m_TextValueFormat = "0";
    // Start is called before the first frame update
    
    
    void Start()
    {
        healthSlider = GameObject.FindObjectOfType<HealthGlobeControl>().healthSlider;
        //healingBarFront = GameObject.FindGameObjectWithTag("HealFront").GetComponent<Image>();
    
        currentLife = healthSlider.value; // Compute current life
        currentHeal = bar.fillAmount; // Compute current heal
    }

    // Update is called once per frame
    void Update()
    {
        if (isHealing)
        {
            if (bar.fillAmount > currentHeal)
            {
                GameManager.Instance.player.GetComponent<Health>().RegenLifePlayer(Time.deltaTime);
                
                bar.fillAmount -= healDecrease * Time.deltaTime;
                healthSlider.value += lifeIncrease * Time.deltaTime;
            }
            else
            {
                bar.fillAmount = currentHeal;
                healthSlider.value = currentLife;
                isHealing = false;
            }
        }

        if (isPickingPlant)
        {
            if (bar.fillAmount < currentHeal)
            {
                bar.fillAmount += healIncrease * Time.deltaTime;
            }
            else
            {
                bar.fillAmount = currentHeal;
                isPickingPlant = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            UsePlant();
        }
    }

    public void UsePlant()
    {
        missingLife = maxLife - healthSlider.value; // Compute missing life
        healDecrease = Mathf.Min(currentHeal, missingLife);
        lifeIncrease = Mathf.Min(currentHeal, missingLife);

        if (missingLife > 0f && currentHeal > 0f)
        {
            currentHeal -= healDecrease;
            currentLife += lifeIncrease;
        }
        else
        {
            GameManager.Instance.FeedbackMessage.SetMessage(currentHeal <= 0f
                ? "Pas assez de ressource"
                : "Pas besoin de soin");

            isHealing = false;
            return;
        }
        isHealing = true;
        
        var player = GameManager.Instance.player;
        var questList = player.GetComponent<QuestManager>();

        var evaluatedQuest = questList.Evaluate("HasQuest", "PlantQuest");
        if (evaluatedQuest != null)
        {
            questList.CompleteGoal(questList.GetQuestByName("PlantQuest"), "1");
        }
        player.GetComponent<PlayerFX>().PlayEatPlant();
        
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
