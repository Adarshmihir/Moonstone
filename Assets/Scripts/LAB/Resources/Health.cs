using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Control;
using Core;
using UI.DamageText;
using UnityEngine;
using UnityEngine.AI;

namespace ResourcesHealth
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealthPoints = 100f;
        [SerializeField] private float destroyTime = 5f;
        [SerializeField] private float destroyTimeWithLoot = 100f;

        private LifeBarController _lifeBarController;
        private DamageTextSpawner _damageTextSpawner;
        private Animator _animator;
        private ActionScheduler _actionScheduler;
        private CapsuleCollider _capsuleCollider;
        private NavMeshAgent _navMeshAgent;
        private AIController _aiController;
        private CombatTarget _combatTarget;
        private Coroutine _death;
        private Spawner spawner;

        public float HealthPoints { get; private set; }
        public float MaxHealthPoints => maxHealthPoints;

        public bool IsDead { get; private set; }

        private List<string> Dots { get; } = new List<string>();

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
            _combatTarget = GetComponent<CombatTarget>();

            if (_lifeBarController != null)
            {
                _lifeBarController.InitLifeBar();
            }
        }

        private void Update()
        {
            if (_combatTarget == null || !IsDead || _combatTarget.Items.Any()) return;
            
            if (_death != null)
            {
                StopCoroutine(_death);
            }

            StartCoroutine(DestroyEnemy(destroyTime));
        }

        public void TakeDamage(float damage, bool criticalHit, Fighter attacker )
        {
            damage = criticalHit ? damage * 2 : damage;
            HealthPoints = Mathf.Max(HealthPoints - damage, 0);

            if (_lifeBarController != null)
            {
                _lifeBarController.UpdateLifeBar();
            }

            if(CompareTag("Player"))
            {
                var healthPlayer = FindObjectOfType<HealthGlobeControl>();
                healthPlayer.StopRegen();
                healthPlayer.healthSlider.value -= (damage / maxHealthPoints);
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

        public void GiveLife(float lifeAmount)
        {
            HealthPoints = Mathf.Min(HealthPoints + lifeAmount, MaxHealthPoints);

            if (_lifeBarController != null)
            {
                _lifeBarController.UpdateLifeBar();
            }

            if (_damageTextSpawner != null)
            {
                _damageTextSpawner.Spawn(lifeAmount, DamageType.Heal);
            }
        }

        public void ResetLifePlayer()
        {
            if (CompareTag("Player"))
            {
                HealthPoints = maxHealthPoints;
                _animator.ResetTrigger("die");
                _animator.Play("Locomotion");
                IsDead = false;
                _capsuleCollider.enabled = true;
                _navMeshAgent.enabled = true;
                HealthGlobeControl healhPlayer = GameObject.FindObjectOfType<HealthGlobeControl>();
                healhPlayer.healthSlider.value = 1;

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

        public void RegenLifePlayer(float regenRate)
        {
            HealthPoints = Mathf.Min(HealthPoints + maxHealthPoints * regenRate, maxHealthPoints);
        }

        public void setSpawner(Spawner spawner)
        {
            this.spawner = spawner;
        }

        public void addHealthPlayer(float bonusHealth)
        {
            this.maxHealthPoints += bonusHealth;
            this.HealthPoints += bonusHealth;

            HealthGlobeControl healhPlayer = GameObject.FindObjectOfType<HealthGlobeControl>();
            healhPlayer.healthSlider.value = healhPlayer.healthSlider.value + (bonusHealth / maxHealthPoints);
        }

        private void Die()
        {
            if (IsDead) return;

            IsDead = true;

            if (_animator != null)
            {
                _animator.SetTrigger("die");
            }
            
            if (_actionScheduler != null)
            {
                _actionScheduler.CancelCurrentAction();
            }

            //_capsuleCollider.enabled = false;
            //_navMeshAgent.enabled = false;

            Enemy enemy = gameObject.GetComponent<Enemy>();
            if (enemy)
            {
                GameManager.Instance.uiManager.LevelManagerGO.GetComponent<LevelManager>().levelWindow.levelSystem.AddExperience(enemy.XpValue);
                if (spawner != null)
                {
                    Debug.Log(spawner);
                    spawner.RemoveObject(gameObject);
                }
            }
            
            if (_death != null)
            {
                StopCoroutine(_death);
            }

            if (!CompareTag("Player"))
			{
                Debug.Log(GetComponent<CombatTarget>().ListLoot.Count);
                var gold = (int) Random.Range(GetComponent<CombatTarget>().MINGold, GetComponent<CombatTarget>().MAXGold);
                GameManager.Instance.player.inventory.gold += gold;
                _death = StartCoroutine(DestroyEnemy(_combatTarget.ListLoot.Count > 0 ? destroyTimeWithLoot : destroyTime));
				PurgeManager.Instance.killedCount += 1;

                if (GetComponent<AISpellController>() == null) return;
                
                GameManager.Instance.Boss.SetActive(true);
            }
            else
            {
                StartCoroutine(RespawnPlayer());
            }
        }

        private IEnumerator RespawnPlayer()
        {
            yield return new WaitForSeconds(1f);
            
            var loaderAnimator = GameManager.Instance.LoaderAnimator;
            loaderAnimator.SetTrigger("startFade");

            yield return new WaitForSeconds(1f);
            
            GameManager.Instance.RespawnPlayer();
            
            yield return new WaitForSeconds(1f);
            
            loaderAnimator.SetTrigger("endFade");
        }

        public void TakeDot(Spell spell, Fighter fighter)
        {
            if (!(spell.DotCount > 0) || Dots.Contains(spell.name)) return;

            Dots.Add(spell.name);
            StartCoroutine(StartDot(spell, fighter));
        }

        private IEnumerator StartDot(Spell spell, Fighter fighter)
        {
            for (var i = 0; i < spell.DotCount; i++)
            {
                yield return new WaitForSeconds(spell.DotTick);

                if (spell.SpellEffect == SpellEffect.Heal)
                {
                    GiveLife(spell.DotDamage);
                }
                else
                {
                    TakeDamage(spell.DotDamage, false, fighter);
                }
            }

            Dots.Remove(spell.name);
        }

        private IEnumerator DestroyEnemy(float timer)
        {
            yield return new WaitForSeconds(timer);

            _capsuleCollider.enabled = false;
            _navMeshAgent.enabled = false;

            if(transform.parent.gameObject != null)
                Destroy(transform.parent.gameObject);
            else
                Destroy(gameObject);
        }

    }
}
