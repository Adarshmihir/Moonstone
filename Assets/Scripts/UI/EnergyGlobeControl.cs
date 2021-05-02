using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGlobeControl : MonoBehaviour
{
    public Slider energySlider;

    public int maxEnergy = 100;
    public float energy;
    void Start()
    {
        energy = maxEnergy;
        energySlider = GetComponent<Slider>();
        energySlider.value = 1;
    }

    public bool HasEnoughEnergy(float cost)
    {
        if (energySlider.value * maxEnergy >= cost) return true;
        
        GameManager.Instance.FeedbackMessage.SetMessage("Pas assez de mana");
        return false;
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
            energy -= cost;
            energySlider.value -= cost / maxEnergy;
            return true;
        }
    }

    public void RestoreEnergy(float regen)
    {
        if (energySlider.value + regen/maxEnergy > 1)
        {
            energy = maxEnergy;
            energySlider.value = 1;
        }
        else
        {
            energy += regen;
            energySlider.value += regen / maxEnergy;
        }
    }

    public void addEnergyPlayer(int bonusEnergy)
    {
        maxEnergy += bonusEnergy;
        energy += bonusEnergy;

        this.energySlider.value = energy / maxEnergy;

    }

}
