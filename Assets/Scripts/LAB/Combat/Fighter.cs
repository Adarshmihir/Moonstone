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

        private Health _target;
        private float _timeSinceLastAttack = Mathf.Infinity;

        public Health Target => _target;

        // Start is called before the first frame update
        private void Start()
        {
            SpawnWeapon();
        }
    
        // Update is called once per frame
        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null || _target.IsDead)
            {
                StopAttack();
                return;
            }

            // Check if target is not too far
            if (!GetIsInRange())
            {
                // Move towards the target until it is close enough
                GetComponent<Mover>().MoveTo(_target.transform.position);
            }
            else
            {
                // Cancel movement action and start attack
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        private void SpawnWeapon()
        {
            if (weapon == null) return;
            
            // Spawn weapon(s) in hand(s)
            var animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);

            
        }
    
        private void AttackBehavior()
        {
            // Rotate the character in direction of the target
            transform.LookAt(_target.transform);
            // Check if weapon cooldown is elapsed and if target if visible
            if (_target.HealthPoints<=0) return;

            // Start attack
            TriggerAttack();
            _timeSinceLastAttack = 0;
        }

        private void TriggerAttack()
        {
            // Start random attack animation
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().speed = weapon.AttackSpeed;
            var attackAnimationToPlay = Random.Range(0, weapon.AnimationTotalPlayChance);
            if (attackAnimationToPlay < weapon.AnimationOnePlayChance)
            {
                GetComponent<Animator>().SetTrigger("attack1");
            }
            else if (attackAnimationToPlay < weapon.AnimationOnePlayChance + weapon.AnimationTwoPlayChance)
            {
                GetComponent<Animator>().SetTrigger("attack2");
            }
            else
            {
                GetComponent<Animator>().SetTrigger("attack3");
            }
           
        }
        
        // Animation event : Attack
        private void Hit()
        {

            if (_target == null) return;

            // Unarmed attack and One Hand armed attack
            if (weapon.WeaponType == WeaponType.Unarmed || weapon.WeaponType == WeaponType.OneHanded )
            {
                // Check if target is in front of character and visible
                if (!GetIsInFieldOfView(_target.transform) || !GetIsAccessible(_target.transform)) return;
                
                // Deal damage
                _target.TakeDamage(weapon.CalculateDamageWeapon(), Random.Range(0, 100) / 100f < criticalChance);
            }
            // Two hand Armed attack 
            else
            {
                // Get all enemies in front of character depending on weapon radius and weapon range
                var spherePosition = (transform.position + _target.transform.position) / 2;
                var colliders = Physics.OverlapSphere(spherePosition, weapon.WeaponRange);
                foreach (var target in colliders)
                {
                    // Check if the target has health, is in front of character and visible
                    if (!CanAttack(target.gameObject) || !GetIsInFieldOfView(target.transform) || !GetIsAccessible(target.transform) || CompareTag(target.tag)) continue;
                    
                    Hit(target.GetComponent<Health>());
                }
            }
        }

        private void Hit(Health target)
        {
            if (target == null) return;
            
            // Deal damage
            target.TakeDamage(weapon.CalculateDamageWeapon(), Random.Range(0, 100) / 100f < criticalChance);
        }
    
        private bool GetIsInRange()
        {
            // Check if the target is in range of weapon
            return Vector3.Distance(transform.position, _target.transform.position) < weapon.WeaponRange;
        }

        private bool GetIsInFieldOfView(Transform target)
        {
            // Check if the target is in front of character
            return Vector3.Angle(target.position - transform.position, transform.forward) <= weapon.WeaponRadius;
        }

        private bool GetIsAccessible(Transform target)
        {
            // Check if target is visible from the character
            var position = transform.position;
            Physics.Raycast(position, target.position - position, out var hit);

            return true; //hit.collider == null || hit.collider.GetComponent<Health>() != null;
        }

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
            _target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            // Stop attack action
            StopAttack();
            // Remove target
            _target = null;
        }
        
        private void StopAttack()
        {
            // Stop attack animation
            GetComponent<Animator>().speed = 1;
            GetComponent<Animator>().ResetTrigger("attack1");
            GetComponent<Animator>().ResetTrigger("attack2");
            GetComponent<Animator>().ResetTrigger("attack3");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}
