using UnityEngine;
using UnityEngine.UI;

public class FillPlantsBar : MonoBehaviour {

    
    
    public Image healingBarFront; // frontend value of heal
    public float currentHeal; // computed value of heal
    public float healIncrease;
    public float healDecrease;
    
    public float minHeal = 0f;
    public float maxHeal = 1f;

    public float minLife = 0f;
    public float maxLife = 1f;
    public Image lifeBar; // frontend value of hp
    public float currentLife; // computed value of hp
    public float missingLife;
    public float lifeIncrease;
    public float lifeDecrease;
    
    public bool isHealing = false;
    public bool isPickingPlant = false;
    public float healQty = 0f; // value that will be added / substracted to healbar & lifebar

    public Button decreaseButton;

    // Start is called before the first frame update
    void Start() {
        lifeBar = GameObject.FindGameObjectWithTag("LifeBar").GetComponent<Image>();
        healingBarFront = GameObject.FindGameObjectWithTag("HealFront").GetComponent<Image>();
        lifeBar.gameObject.SetActive(false);
        decreaseButton = GetComponentInChildren<Button>();
        
        currentLife = lifeBar.fillAmount; // Compute current life
        currentHeal = healingBarFront.fillAmount; // Compute current heal
    }

    // Update is called once per frame
    void Update() {
        if (isHealing) {
            if(healingBarFront.fillAmount > currentHeal) {
                healingBarFront.fillAmount -= healDecrease * Time.deltaTime;
                lifeBar.fillAmount += lifeIncrease * Time.deltaTime;
            }
            else {
                healingBarFront.fillAmount = currentHeal;
                lifeBar.fillAmount = currentLife;
                isHealing = false;
            }
        }
        
        if (isPickingPlant) {
            if(healingBarFront.fillAmount < currentHeal) {
                healingBarFront.fillAmount += healIncrease * Time.deltaTime;
            }
            else {
                healingBarFront.fillAmount = currentHeal;
                isPickingPlant = false;
            }
        }

        if(lifeBar.fillAmount <= 0f)
            Debug.Log("You're dead !");
    }

    public void UsePlant() {
        Debug.Log("Using plant !");
        missingLife = maxLife - lifeBar.fillAmount; // Compute missing life
        healDecrease = Mathf.Min(currentHeal, missingLife);
        lifeIncrease = Mathf.Min(currentHeal, missingLife);
        
        if (missingLife > 0f && currentHeal > 0f) { // If we need to regen
            currentHeal -= healDecrease;
            currentLife += lifeIncrease;
        }
        else {
            if(currentHeal <= 0f)
                Debug.Log("Not enough heal to regen your life !"); // No healing available
            else
                Debug.Log("You don't need to heal !");

            isHealing = false;
            return;
        }
        isHealing = true;
    }
    
    public void TakeDamage(float amount) {
        if (currentLife - amount >= 0f)
            currentLife -= amount;
        else {
            currentLife = 0f;
        }
        lifeBar.fillAmount = currentLife;
    }

    public void PickPlant(float healAmount) {
        if (currentHeal + healAmount < 1f) {
            healIncrease = healAmount;
            currentHeal += healIncrease;
        }
        else {
            healIncrease = 1f - currentHeal;
            currentHeal = 1f;
        }
        isPickingPlant = true;
    }
}
