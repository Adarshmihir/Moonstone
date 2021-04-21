using System.Collections;
using System.Collections.Generic;
using ResourcesHealth;
using UnityEngine;
using UnityEngine.UI;

public class HealthGlobeControl : MonoBehaviour
{
    public Slider healthSlider;
    public float regenSpeed;

    public bool regenering;
    public Coroutine coroutineRegen;
    void Start()
    {
        healthSlider = GetComponent<Slider>();
        healthSlider.value = 1;
        regenering = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (regenering)
        {
            if (healthSlider.value < 1)
            {
                healthSlider.value += (regenSpeed * Time.deltaTime);
                GameManager.Instance.player.GetComponent<Health>().RegenLifePlayer(regenSpeed * Time.deltaTime);
            }
            if (healthSlider.value > 1)
            {
                healthSlider.value = 1;
                regenering = false;
            }
        }
    }

    public void StopRegen()
    {
        regenering = false;
        if (coroutineRegen != null)
        {
            StopCoroutine(coroutineRegen);
        }
        coroutineRegen = StartCoroutine(StartRegenDelay());
    }

    private IEnumerator StartRegenDelay()
    {
        yield return new WaitForSeconds(5);
        regenering = true;
        coroutineRegen = null;
    }
}