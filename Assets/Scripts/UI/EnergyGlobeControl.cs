using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGlobeControl : MonoBehaviour
{
    public Slider energySlider;

    public int maxEnergy = 100;

    void Start()
    {
        energySlider = GetComponent<Slider>();
        energySlider.value = 1;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            UseEnergy(20);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            RestoreEnergy(10);
        }
    }

    public bool HasEnoughEnergy(float cost)
    {
        return energySlider.value * maxEnergy >= cost;
    }

    public bool UseEnergy(float cost)
    {
        if (energySlider.value*maxEnergy < cost)
        {
            Debug.Log("Pas assez de mana");
            return false;
        }
        else
        {
            energySlider.value -= cost / maxEnergy;
            return true;
        }
    }

    public void RestoreEnergy(float regen)
    {
        if (energySlider.value + regen/maxEnergy > 1)
        {
            energySlider.value = 1;
        }
        else
        {
            energySlider.value += regen / maxEnergy;
        }
    }

    public void addEnergyPlayer(int bonusEnergy)
    {
        maxEnergy += bonusEnergy;
        this.energySlider.value += bonusEnergy / maxEnergy;
    }

}
