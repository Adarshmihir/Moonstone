using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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
        [SerializeField] private float attackspeed = 1f;
        [SerializeField] private float weaponMinDamage = 5f;
        [SerializeField] private float weaponMaxDamage = 10f;
        [SerializeField] [Range(0f, 180f)] private float weaponRadius = 45f;

        [SerializeField] private AnimatorOverrideController animatorOverride;
        [SerializeField] private GameObject rightHandWeaponPrefab;
        [SerializeField] private GameObject leftHandWeaponPrefab;

        [SerializeField] [Range(0f, 1f)] private float animationOnePlayChance;
        [SerializeField] [Range(0f, 1f)] private float animationTwoPlayChance;
        [SerializeField] [Range(0f, 1f)] private float animationThreePlayChance;

        // Damage flat and percent of stat
        [SerializeField] public float weaponDamageFlat = 5f;
        [SerializeField] [Range(0, 5)]private float weaponDamagePercent = 0.5f;
        [SerializeField] private StatTypes CurrentStatUsing = StatTypes.Strength;

        public List<StatModifier> StatModifiers;

        [SerializeField] public int ironToBuild;
        [SerializeField] public int copperToBuild;
        [SerializeField] public int silverToBuild;

        public float WeaponRange => weaponRange;
        public float WeaponRadius => weaponRadius;
        public float AttackSpeed => attackspeed;

        public float WeaponDamage => (float) Math.Round(Random.Range(weaponMinDamage, weaponMaxDamage), MidpointRounding.AwayFromZero);
        public WeaponType WeaponType => weaponType;

        public float AnimationOnePlayChance => animationOnePlayChance;
        public float AnimationTwoPlayChance => animationTwoPlayChance;
        public float AnimationTotalPlayChance => animationOnePlayChance + animationTwoPlayChance + animationThreePlayChance;

        private void OnEnable()
        {
            this.assignStatModifiers();
        }

        public Tuple<GameObject, GameObject> Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator, GameObject rightClone, GameObject leftClone)
        {
            DestroyWeapon(rightClone, leftClone);

            if (rightHandWeaponPrefab != null)
            {
                rightClone = Instantiate(rightHandWeaponPrefab, rightHandTransform);
            }

            if (leftHandWeaponPrefab != null)
            {
                leftClone= Instantiate(leftHandWeaponPrefab, leftHandTransform);
            }

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }

            return Tuple.Create(rightClone, leftClone);
        }

        private void DestroyWeapon(GameObject rightClone, GameObject leftClone)
        {
            
            if (rightClone != null) {
                Destroy(rightClone);
            }

            if (leftClone != null)
                Destroy(leftClone);
        }

        public override void Use() {
            base.Use();
            SwapWeapons();
        }

        // /!\ SI
        private void SwapWeapons()
        {
            var fighter = GameManager.Instance.player.GetComponent<Fighter>();
            var animator = GameManager.Instance.player.GetComponent<Animator>();
            
            (fighter.rightClone, fighter.leftClone) = Spawn(fighter.rightHandTransform, fighter.leftHandTransform, animator, fighter.rightClone, fighter.leftClone);
            
            Weapon oldItem = (Weapon)EquipmentManager.instance.Equip(this);
            if (oldItem != null)
            {
                foreach (var mod in oldItem.StatModifiers)
                {
                    GameManager.Instance.player.RemoveModifier(mod);
                }
            }
            foreach (var mod in StatModifiers)
            {
                GameManager.Instance.player.AddModifier(mod);
            }
            GameManager.Instance.uiManager.InventoryGO.GetComponentInChildren<chooseEquipSlot>().addEquipment(this);
            
            GameManager.Instance.player.GetComponent<FighterSpell>().UpdateSpell(Spell, CastSource.Weapon);
            
            // Remove it from the inventory
            RemoveFromInventory();

            fighter.weapon = this;
        }

        // Function calculate Dmg with flat dmg of weapon  + percent of stat of player
        public float CalculateDamageWeapon()
        {
            float statValue = GameManager.Instance.player.stats.Find(x => x.StatName == CurrentStatUsing).charStat.BaseValue;
            return Mathf.Round(weaponDamageFlat + (statValue * weaponDamagePercent));
        }

        public string SelectAnAnimation()
        {
            var animationTotalPlayChance = animationOnePlayChance + animationTwoPlayChance + animationThreePlayChance;
            var attackAnimationToPlay = Random.Range(0, animationTotalPlayChance);

            if (attackAnimationToPlay < animationOnePlayChance) return "attack1";

            return attackAnimationToPlay < animationOnePlayChance + animationTwoPlayChance ? "attack2" : "attack3";
        }

        public void assignStatModifiers()
        {
            StatModifiers.Clear();
            for (int i = 0; equipementMods != null && i < equipementMods.Length; i++)
            {
                StatModifier newMod = StatModifier.CreateInstance(equipementMods[i].value, equipementMods[i].modType, this, equipementMods[i].statType);
                StatModifiers.Add(newMod);
            }
        }
    }
}
