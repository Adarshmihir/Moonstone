using System;
using Resources;
using UnityEngine;

namespace Combat
{
    public enum SpellType
    {
        UniqueEffect,
        ContactEffect,
        ZoneEffect
    }
    
    // TODO : Enum spell effect (heal, one time damage or dot)
    // TODO : Spell type (fire, light, etc.)
    
    [CreateAssetMenu(fileName = "Spell", menuName = "Moonstone/New Spell", order = 0)]
    public class Spell : ScriptableObject
    {
        [SerializeField] private SpellType spellType;
        [SerializeField] private Projectile projectile;
        [SerializeField] private float cooldown = 1f;
        [SerializeField] private float spellDamage = 5f;
        // For UniqueEffect, ContactEffect and ZoneEffect
        [SerializeField] private float spellRange = 2f;
        // For ZoneEffect
        [SerializeField] private float spellZoneArea = 2f;
        // For ContactEffect
        [SerializeField] [Range(0f, 180f)] private float damageRadius = 45f;

        [field: NonSerialized]
        public float CurrentCooldown { get; set; }

        public void Launch(Transform output, Health target)
        {
            if (projectile == null)
            {
                // TODO : Call launch AOE
            }
            else
            {
                LaunchProjectile(output, target);
            }
        }

        private void LaunchProjectile(Transform output, Health target)
        {
            var projectileInstance = Instantiate(projectile, output.position, Quaternion.identity);
            projectileInstance.Target = target;
        }

        public void ResetCooldown()
        {
            CurrentCooldown = cooldown;
        }

        public void PutOnCooldown()
        {
            CooldownManager.instance.StartCooldown(this);
        }

        public bool IsSpellOnCooldown()
        {
            return CurrentCooldown > 0;
        }
    }
}
