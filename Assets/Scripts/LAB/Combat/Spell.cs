using System;
using UnityEngine;

namespace Combat
{
    public enum SpellType
    {
        UniqueEffect,
        ContactEffect,
        ZoneEffect
    }

    public enum SpellEffect
    {
        Damage,
        Heal
    }
    
    // TODO : Enum spell effect (heal, one time damage or dot)
    // TODO : Spell type (fire, light, etc.)
    
    [CreateAssetMenu(fileName = "Spell", menuName = "Moonstone/New Spell", order = 0)]
    public class Spell : ScriptableObject
    {
        [SerializeField] private SpellType spellType;
        [SerializeField] private SpellEffect spellEffect;
        
        [SerializeField] private Projectile projectile;
        
        [SerializeField] private float spellDamage = 5f;
        [SerializeField] public float spellCost = 20;
        [SerializeField] private float dotDamage;
        [SerializeField] private float dotCount;
        [SerializeField] private float dotTick;
        [SerializeField] private float cooldown = 1f;
        
        [SerializeField] private float canalisationTimer = 1f;
        
        [SerializeField] private bool isAnimated = true;
        [SerializeField] private GameObject particleEffect;
        //[SerializeField] private Vector3 particleSize;
        [SerializeField] private GameObject particleEffectImpact;
        //[SerializeField] private Vector3 particleSizeImpact;
        
        [SerializeField] private Texture spellIcon;
        
        // For UniqueEffect, ContactEffect and ZoneEffect
        [SerializeField] private float spellRange = 2f;
        // For ZoneEffect
        [SerializeField] private float spellZoneArea = 2f;
        // For ContactEffect
        [SerializeField] [Range(0f, 180f)] private float damageRadius = 45f;

        [field: NonSerialized]
        public float CurrentCooldown { get; set; }
        public bool IsAnimated => isAnimated;
        public float SpellDamage => spellDamage;
        public Texture SpellIcon => spellIcon;
        public float Cooldown => cooldown;
        public float SpellRange => spellRange;
        public float SpellZoneArea => spellZoneArea;
        public float DamageRadius => damageRadius;
        public float DotDamage => dotDamage;
        public float DotCount => dotCount;
        public float DotTick => dotTick;
        public GameObject ParticleEffectImpact => particleEffectImpact;
        //public Vector3 ParticleSizeImpact => particleSizeImpact;
        public SpellType SpellType => spellType;
        public SpellEffect SpellEffect => spellEffect;
        public float CanalisationTimer => canalisationTimer;

        public void Launch(Transform output, Fighter attacker)
        {
            LaunchProjectile(output, attacker);
        }

        private void LaunchProjectile(Transform output, Fighter attacker)
        {
            var projectileInstance = Instantiate(projectile, output.position, Quaternion.identity);
            
            //UpdateParticleSize();
            //SetParticle(projectileInstance);
            
            projectileInstance.Spell = this;
            projectileInstance.Attacker = attacker;
            projectileInstance.Output = output;
            projectileInstance.StartCast();

            SetParticle(projectileInstance);
            //UpdateParticleSize();
        }

        private void SetParticle(Component projectileInstance)
        {
            var particle = Instantiate(particleEffect, projectileInstance.transform.position, Quaternion.identity);
            
            particle.transform.parent = projectileInstance.transform;
            //particle.transform.localScale = particleSize;
        }

        /*private void UpdateParticleSize()
        {
            if (spellType != SpellType.ZoneEffect) return;
            
            particleSize = new Vector3(spellZoneArea, spellZoneArea, spellZoneArea);
            particleSizeImpact = new Vector3(spellZoneArea, spellZoneArea, spellZoneArea);
        }*/

        public void ResetCooldown()
        {
            CurrentCooldown = cooldown;
        }

        public void PutOnCooldown(CastSource castSource)
        {
            CooldownManager.instance.StartCooldown(this, castSource);
        }

        public bool IsSpellOnCooldown()
        {
            return CurrentCooldown > 0;
        }
    }
}
