using Core;
using Movement;
using Resources;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] [Range(0f, 1f)] private float criticalChance = 0.5f;

        [SerializeField] private Weapon weapon;
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private Transform leftHandTransform;

        private Mover _mover;
        private Animator _animator;
        private float _timeSinceLastAttack = Mathf.Infinity;

        public Health Target { get; private set; }

        // Start is called before the first frame update
        private void Start()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
            
            SpawnWeapon();
        }
    
        // Update is called once per frame
        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (Target == null || Target.IsDead) return;

            // Check if target is not too far
            if (!GetIsInRange())
            {
                // Move towards the target until it is close enough
                _mover.MoveTo(Target.transform.position);
            }
            else
            {
                // Cancel movement action and start attack
                _mover.Cancel();
                AttackBehavior();
            }
        }

        private void SpawnWeapon()
        {
            if (weapon == null) return;
            
            // Spawn weapon(s) in hand(s)
            weapon.Spawn(rightHandTransform, leftHandTransform, _animator);
        }
    
        private void AttackBehavior()
        {
            // Rotate the character in direction of the target
            transform.LookAt(Target.transform);
            // Check if weapon cooldown is elapsed and if target if visible
            if (!(_timeSinceLastAttack > weapon.TimeBetweenAttack)/* || !GetIsAccessible(target.transform)*/) return;

            // Start attack
            TriggerAttack();
            _timeSinceLastAttack = 0;
        }

        private void TriggerAttack()
        {
            // Start random attack animation
            _animator.ResetTrigger("stopAttack");
            _animator.SetTrigger(weapon.SelectAnAnimation());
        }
        
        // Animation event : Attack
        private void Hit()
        {
            if (Target == null) return;

            // Unarmed attack
            if (weapon.WeaponType == WeaponType.Unarmed)
            {
                // Check if target is in front of character and visible
                if (!GetIsInFieldOfView(Target.transform)/* || !GetIsAccessible(target.transform)*/) return;
                
                // Deal damage
                Target.TakeDamage(weapon.WeaponDamage, Random.Range(0, 100) / 100f < criticalChance);
            }
            // Armed attack
            else
            {
                // Deal damage to all enemies around
                AttackAllEnemiesAround();
            }
        }

        private void AttackAllEnemiesAround()
        {
            // Get all enemies in front of character depending on weapon radius and weapon range
            var spherePosition = (transform.position + Target.transform.position) / 2;
            var colliders = Physics.OverlapSphere(spherePosition, weapon.WeaponRange);
            foreach (var newTarget in colliders)
            {
                // Check if the target has health, is in front of character and visible
                if (!CanAttack(newTarget.gameObject) || !GetIsInFieldOfView(newTarget.transform)/* || !GetIsAccessible(target.transform)*/ || CompareTag(newTarget.tag)) continue;
                    
                Hit(newTarget.GetComponent<Health>());
            }
        }

        private void Hit(Health targetHealth)
        {
            if (targetHealth == null) return;
            
            // Deal damage
            targetHealth.TakeDamage(weapon.WeaponDamage, Random.Range(0, 100) / 100f < criticalChance);
        }
    
        private bool GetIsInRange()
        {
            // Check if the target is in range of weapon
            return Vector3.Distance(transform.position, Target.transform.position) < weapon.WeaponRange;
        }

        private bool GetIsInFieldOfView(Transform targetTransform)
        {
            // Check if the target is in front of character
            var charTransform = transform;
            return Vector3.Angle(targetTransform.position - charTransform.position, charTransform.forward) <= weapon.WeaponRadius;
        }

        /*private bool GetIsAccessible(Transform target)
        {
            // Check if target is visible from the character
            var position = transform.position;
            Physics.Raycast(position, target.position - position, out var hit);

            return true; //hit.collider == null || hit.collider.GetComponent<Health>() != null;
        }*/

        public static bool CanAttack(GameObject combatTarget)
        {
            // Check if the target has health and is not already dead
            if (combatTarget == null) return false;
            
            var targetHealth = combatTarget.GetComponent<Health>();
            return targetHealth != null && !targetHealth.IsDead;
        }
    
        public void Attack(GameObject combatTarget)
        {
            // Start attack action
            GetComponent<ActionScheduler>().StartAction(this);
            // Define target
            Target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            // Stop attack action
            StopAttack();
            // Remove target
            Target = null;
        }
        
        private void StopAttack()
        {
            // Stop attack animation
            _animator.ResetTrigger("attack1");
            _animator.ResetTrigger("attack2");
            _animator.ResetTrigger("attack3");
            _animator.SetTrigger("stopAttack");
        }
    }
}
