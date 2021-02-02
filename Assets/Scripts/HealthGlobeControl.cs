using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthGlobeControl : MonoBehaviour
{
    public Slider healthSlider;
    public float regenSpeed;
    public bool regenering;

    void Start()
    {
        healthSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(regenering)
        {
            if(healthSlider.value < 1)
            {
                healthSlider.value += (regenSpeed * Time.deltaTime);
            }
            if(healthSlider.value > 1)
            {
                healthSlider.value = 1;
                regenering = false;
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            healthSlider.value = healthSlider.value - 0.1f;
        }
        if (Input.GetMouseButtonDown(1))
        {
            regenering = true;
        }
    }
}
