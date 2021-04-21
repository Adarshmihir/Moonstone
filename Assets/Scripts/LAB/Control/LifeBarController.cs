using Combat;
using ResourcesHealth;
using UnityEngine;
using UnityEngine.UI;

namespace Control
{
    [RequireComponent(typeof(Health))]
    public class LifeBarController : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private GameObject healthBarUI;

        private Health _health;
        private Fighter _fighter;
        private AIController _aiController;

        // Start is called before the first frame update
        private void Start()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _aiController = GetComponent<AIController>();
        }
        
        // Update is called once per frame
        private void Update()
        {
            healthSlider.gameObject.SetActive(_aiController != null && _aiController.DistanceToPlayer() && _health.HealthPoints > 0);

            if (Camera.main == null) return;

            var rotation = Camera.main.transform.rotation;
            healthBarUI.transform.LookAt(healthBarUI.transform.position + rotation *- Vector3.back, rotation *- Vector3.down);
        }

        public void UpdateLifeBar()
        {
            if (healthSlider == null || _health == null) return;
            
            healthSlider.value = _health.HealthPoints / _health.MaxHealthPoints;
        }

        public void InitLifeBar()
        {
            if (healthSlider == null || _health == null) return;
        
            healthSlider.value = _health.HealthPoints / _health.MaxHealthPoints;
            healthSlider.gameObject.SetActive(false);
        }
    }
}
