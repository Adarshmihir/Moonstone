using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Combat
{
    public enum WeaponType
    {
        Unarmed,
        OneHanded,
        TwoHanded
    }
    
    [CreateAssetMenu(fileName = "Weapon", menuName = "Moonstone/New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttack = 1f;
        [SerializeField] private float weaponMinDamage = 5f;
        [SerializeField] private float weaponMaxDamage = 10f;
        [SerializeField] [Range(0f, 180f)] private float weaponRadius = 45f;
        
        [SerializeField] private AnimatorOverrideController animatorOverride;
        [SerializeField] private GameObject rightHandWeaponPrefab;
        [SerializeField] private GameObject leftHandWeaponPrefab;
        
        [SerializeField] [Range(0f, 1f)] private float animationOnePlayChance;
        [SerializeField] [Range(0f, 1f)] private float animationTwoPlayChance;
        [SerializeField] [Range(0f, 1f)] private float animationThreePlayChance;

        public float WeaponRange => weaponRange;
        public float WeaponRadius => weaponRadius;
        public float TimeBetweenAttack => timeBetweenAttack;

        public float WeaponDamage => (float) Math.Round(Random.Range(weaponMinDamage, weaponMaxDamage), MidpointRounding.AwayFromZero);
        public WeaponType WeaponType => weaponType;
        
        public float AnimationOnePlayChance => animationOnePlayChance;
        public float AnimationTwoPlayChance => animationTwoPlayChance;
        public float AnimationTotalPlayChance => animationOnePlayChance + animationTwoPlayChance + animationThreePlayChance;

        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            if (rightHandWeaponPrefab != null)
            {
                Instantiate(rightHandWeaponPrefab, rightHandTransform);
            }
            
            if (leftHandWeaponPrefab != null)
            {
                Instantiate(leftHandWeaponPrefab, leftHandTransform);
            }

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }
    }
}
