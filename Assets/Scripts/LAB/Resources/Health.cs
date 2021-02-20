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

        private LifeBarController _lifeBarController;
        private DamageTextSpawner _damageTextSpawner;
        private Animator _animator;
        private ActionScheduler _actionScheduler;
        private CapsuleCollider _capsuleCollider;
        private NavMeshAgent _navMeshAgent;

        public float HealthPoints { get; private set; }
        public float MaxHealthPoints => maxHealthPoints;

        public bool IsDead { get; private set; }
        
        // Start is called before the first frame update
        private void Start()
        {
            HealthPoints = maxHealthPoints;
            
            _damageTextSpawner = GetComponentInChildren<DamageTextSpawner>();
            _lifeBarController = GetComponent<LifeBarController>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public void TakeDamage(float damage, bool criticalHit)
        {
            HealthPoints = Mathf.Max(HealthPoints - damage, 0);

            if (_lifeBarController != null)
            {
                _lifeBarController.UpdateLifeBar();
            }

            if (_damageTextSpawner != null)
            {
                _damageTextSpawner.Spawn(damage, criticalHit ? DamageType.Critical : DamageType.Normal);
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
            _animator.SetTrigger("die");
            _actionScheduler.CancelCurrentAction();
            
            _capsuleCollider.enabled = false;
            _navMeshAgent.enabled = false;

            StartCoroutine(DestroyEnemy());
        }

        private IEnumerator DestroyEnemy()
        {
            yield return new WaitForSeconds(destroyTime);
            
            Destroy(gameObject);
        }
    }
}
