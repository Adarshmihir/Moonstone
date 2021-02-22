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
    public class Weapon : Item
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
        public float AnimationThreePlayChance => animationThreePlayChance;
        public float AnimationTotalPlayChance => animationOnePlayChance + animationTwoPlayChance + animationThreePlayChance;

        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            DestroyWeapon();
            
            if (rightHandWeaponPrefab != null)
            {
                GameManager.Instance.player.GetComponent<Fighter>().rightClone = Instantiate(rightHandWeaponPrefab, rightHandTransform);
                //Debug.Log(rightClone.ToString());
            }
            
            if (leftHandWeaponPrefab != null)
            {
                GameManager.Instance.player.GetComponent<Fighter>().leftClone= Instantiate(leftHandWeaponPrefab, leftHandTransform);
            }

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        private void DestroyWeapon() {
            if (GameManager.Instance.player.GetComponent<Fighter>().rightClone != null) {
                GameObject.Destroy(GameManager.Instance.player.GetComponent<Fighter>().rightClone);
            }
            
            if (GameManager.Instance.player.GetComponent<Fighter>().leftClone != null)
                GameObject.Destroy(GameManager.Instance.player.GetComponent<Fighter>().leftClone);
        }

        public override void Use() {
            base.Use();
            SwapWeapons();
            Debug.Log("j'utilise mon arme");
            
        }
        
        private void SwapWeapons() {
            this.Spawn(GameManager.Instance.player.GetComponent<Fighter>().rightHandTransform, GameManager.Instance.player.GetComponent<Fighter>().leftHandTransform, GameManager.Instance.player.GetComponent<Animator>());
        }
    }
}
