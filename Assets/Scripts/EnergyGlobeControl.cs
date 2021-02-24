using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyGlobeControl : MonoBehaviour
{
    public Slider energySlider;

    private int maxEnergy = 100;

    void Start()
    {
        energySlider = GetComponent<Slider>();
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

    public void UseEnergy(float cost)
    {
        if (energySlider.value*maxEnergy < cost)
        {
            //Cannot launch a spell
            Debug.Log("Pas assez de mana");
        }
        else
        {
            energySlider.value -= cost / maxEnergy;
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
}
