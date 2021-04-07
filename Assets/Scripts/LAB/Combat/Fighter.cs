using System;
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

        [SerializeField] public Weapon weapon;
        [SerializeField] public Transform rightHandTransform;
        [SerializeField] public Transform leftHandTransform;

        public GameObject rightClone;
        public GameObject leftClone;

        private Mover _mover;
        private Animator _animator;
        private float _timeSinceLastAttack = Mathf.Infinity;
        
        public Weapon RNGWeapon;

        public Health Target { get; private set; }

        // Start is called before the first frame update
        private void Start()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();

            RNGWeapon = ScriptableObject.CreateInstance<Weapon>();
            SpawnWeapon();
        }

        // Update is called once per frame
        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (Target == null || Target.IsDead) return;

            // Check if target is not too far
            if (!GetIsInRange(Target.transform.position, weapon.WeaponRange))
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
            (rightClone, leftClone) = weapon.Spawn(rightHandTransform, leftHandTransform, _animator, rightClone, leftClone);
        }

        public void ChangeWeaponVisibility(bool visible)
        {
            if (weapon.WeaponType == WeaponType.OneHanded)
            {
                rightHandTransform.GetChild(rightHandTransform.childCount - 1).gameObject.SetActive(visible);
                leftHandTransform.GetChild(leftHandTransform.childCount - 1).gameObject.SetActive(visible);
            }
            else if (weapon.WeaponType == WeaponType.TwoHanded)
            {
                rightHandTransform.GetChild(rightHandTransform.childCount - 1).gameObject.SetActive(visible);
            }
        }

        private void AttackBehavior()
        {
            // Rotate the character in direction of the target
            transform.LookAt(Target.transform);
            // Check if weapon cooldown is elapsed and if target if visible
            if (!(_timeSinceLastAttack > weapon.AttackSpeed)/* || !GetIsAccessible(target.transform)*/) return;

            // Start attack
            TriggerAttack();
            _timeSinceLastAttack = 0;
        }

        private void TriggerAttack()
        {
            // Start random attack animation
            _animator.ResetTrigger("stopAttack");
			_animator.speed = weapon.AttackSpeed;
            _animator.SetTrigger(weapon.SelectAnAnimation());
        }

        // Animation event : Attack
        private void Hit()
        {
            Debug.Log("Hit");
            if (Target == null) return;

            if(Target.tag == "Enemy")
            {
                Debug.Log("Enemy Detected");
                Target.GetComponent<FighterFX>().PlayBleed();
            }

            // Unarmed attack and One Hand armed attack
            if (weapon.WeaponType == WeaponType.Unarmed || weapon.WeaponType == WeaponType.OneHanded )
            {
                // Check if target is in front of character and visible
                if (!GetIsInFieldOfView(Target.transform, weapon.WeaponRadius)/* || !GetIsAccessible(_target.transform)*/) return;

                if (GetComponent<Player>())
                    // Deal damage
                    Target.TakeDamage(weapon.CalculateDamageWeapon(), Random.Range(0, 100) / 100f < criticalChance, this);
                else
                    Target.TakeDamage(weapon.weaponDamageFlat, Random.Range(0, 100) / 100f < criticalChance, this);
            }
            // Two hand Armed attack
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
                if (!CanAttack(newTarget.gameObject) || !GetIsInFieldOfView(newTarget.transform, weapon.WeaponRadius)/* || !GetIsAccessible(target.transform)*/ || CompareTag(newTarget.tag)) continue;

                Hit(newTarget.GetComponent<Health>());
            }
        }

        private void Hit(Health targetHealth)
        {
            if (targetHealth == null) return;

            // Deal damage
            Target.TakeDamage(weapon.CalculateDamageWeapon(), Random.Range(0, 100) / 100f < criticalChance, this);
        }

        public bool GetIsInRange(Vector3 targetPosition, float range)
        {
            // Check if the target is in range of weapon
            return Vector3.Distance(transform.position, targetPosition) < range;
        }

        public bool GetIsInFieldOfView(Transform targetTransform, float radius)
        {
            // Check if the target is in front of character
            var charTransform = transform;
            return Vector3.Angle(targetTransform.position - charTransform.position, charTransform.forward) <= radius;
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
            _animator.speed = 1;
            _animator.ResetTrigger("attack1");
            _animator.ResetTrigger("attack2");
            _animator.ResetTrigger("attack3");
            _animator.SetTrigger("stopAttack");
        }
    }
}
