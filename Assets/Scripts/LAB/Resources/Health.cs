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

        private LifeBarController lifeBarController;
        private DamageTextSpawner damageTextSpawner;
        private Animator animator;
        private ActionScheduler actionScheduler;
        private CapsuleCollider capsuleCollider;
        private NavMeshAgent navMeshAgent;

        public float HealthPoints { get; private set; }
        public float MaxHealthPoints => maxHealthPoints;

        public bool IsDead { get; private set; }
        
        // Start is called before the first frame update
        private void Start()
        {
            HealthPoints = maxHealthPoints;
            
            damageTextSpawner = GetComponentInChildren<DamageTextSpawner>();
            lifeBarController = GetComponent<LifeBarController>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void TakeDamage(float damage, bool criticalHit)
        {
            HealthPoints = Mathf.Max(HealthPoints - damage, 0);

            if (lifeBarController != null)
            {
                lifeBarController.UpdateLifeBar();
            }

            if (damageTextSpawner != null)
            {
                damageTextSpawner.Spawn(damage, criticalHit ? DamageType.Critical : DamageType.Normal);
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
            animator.SetTrigger("die");
            actionScheduler.CancelCurrentAction();
            
            capsuleCollider.enabled = false;
            navMeshAgent.enabled = false;

            StartCoroutine(DestroyEnemy());
        }

        private IEnumerator DestroyEnemy()
        {
            yield return new WaitForSeconds(destroyTime);
            
            Destroy(gameObject);
        }
    }
}
