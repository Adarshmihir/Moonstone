﻿using Resources;
using UnityEngine;
using UnityEngine.UI;

namespace Control
{
    [RequireComponent(typeof(Health))]
    public class LifeBarController : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private GameObject healthBarUI;

        private Health health;

        // Start is called before the first frame update
        private void Start()
        {
            health = GetComponent<Health>();
            healthSlider.value = CalculateHealthSlider();
        }
        
        // Update is called once per frame
        private void Update()
        {
            if (Camera.main == null) return;

            var rotation = Camera.main.transform.rotation;
            healthBarUI.transform.LookAt(healthBarUI.transform.position + rotation *- Vector3.back, rotation *- Vector3.down);
        }

        public void UpdateLifeBar()
        {
            healthSlider.value = CalculateHealthSlider();
            healthBarUI.SetActive(health.HealthPoints > 0);
        }
    
        private float CalculateHealthSlider()
        {
            return health.HealthPoints / health.MaxHealthPoints;
        }
    }
}
