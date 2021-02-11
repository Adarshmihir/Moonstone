using System.Collections;
using Control;
using Core;
using UI.DamageText;
using UnityEngine;
using UnityEngine.AI;

namespace Resources
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealthPoints = 100f;
        [SerializeField] private float destroyTime = 5f;

        public float HealthPoints { get; private set; }
        public float MaxHealthPoints => maxHealthPoints;

        public bool IsDead { get; private set; }
        
        // Start is called before the first frame update
        private void Start()
        {
            HealthPoints = maxHealthPoints;
        }

        public void TakeDamage(float damage, bool criticalHit)
        {
            HealthPoints = Mathf.Max(HealthPoints - damage, 0);

            var lifeBarController = GetComponent<LifeBarController>();
            if (lifeBarController != null)
            {
                lifeBarController.UpdateLifeBar();
            }

            var damageSpawner = GetComponentInChildren<DamageTextSpawner>();
            if (damageSpawner != null)
            {
                damageSpawner.Spawn(damage, criticalHit ? DamageType.Critical : DamageType.Normal);
            }

            if (HealthPoints <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (IsDead) return;

            IsDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;

            StartCoroutine(DestroyEnemy());
        }

        private IEnumerator DestroyEnemy()
        {
            yield return new WaitForSeconds(destroyTime);
            
            Destroy(gameObject);
        }
    }
}
