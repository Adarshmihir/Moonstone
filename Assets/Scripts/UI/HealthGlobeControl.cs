using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthGlobeControl : MonoBehaviour
{
    public Slider healthSlider;
    public float regenSpeed;
    public bool regenerating;

    void Start()
    {
        healthSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (regenerating)
        {
            if (healthSlider.value < 1)
            {
                healthSlider.value += (regenSpeed * Time.deltaTime);
            }
            if (healthSlider.value > 1)
            {
                healthSlider.value = 1;
                regenerating = false;
            }
        }
    }
}