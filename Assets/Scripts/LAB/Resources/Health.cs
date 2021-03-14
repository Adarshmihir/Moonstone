using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combat;
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
        private AIController _aiController;

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
            _aiController = GetComponent<AIController>();
        }

        public void TakeDamage(float damage, bool criticalHit, Fighter attacker)
        {
            damage = criticalHit ? damage * 2 : damage;
            HealthPoints = Mathf.Max(HealthPoints - damage, 0);

            if (_lifeBarController != null)
            {
                _lifeBarController.UpdateLifeBar();
            }

            if (_damageTextSpawner != null)
            {
                // If critical , damage * 2 else normal damage
			    _damageTextSpawner.Spawn(damage, criticalHit ? DamageType.Critical : DamageType.Normal);
            }

            if (HealthPoints <= 0)
            {
                Die();
            }
            else if (_aiController != null && !_aiController.IsGoingHome)
            {
                _aiController.ConfigureTarget(attacker, damage);
            }
        }

        public void RegenLife()
        {
            if (HealthPoints >= maxHealthPoints) return;

            HealthPoints = maxHealthPoints;

            if (_lifeBarController != null)
            {
                _lifeBarController.UpdateLifeBar();
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

            if(!CompareTag("Player"))
                PurgeManager.Instance.killedCount++;
        }

        private IEnumerator DestroyEnemy()
        {
            yield return new WaitForSeconds(destroyTime);

            if(transform.parent.gameObject != null)
                Destroy(transform.parent.gameObject);
            else
                Destroy(gameObject);
        }
    }
}
