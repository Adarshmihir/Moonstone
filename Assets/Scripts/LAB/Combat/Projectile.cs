using System;
using System.Collections;
using Core;
using Resources;
using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;

        private Vector3 _initialPosition;
        private bool _isCasting = true;
        private float _destroyTimer;
        private EnergyGlobeControl _energy;
        
        public Spell Spell { get; set; }
        //public Health Target { get; set; }
        public Fighter Attacker { get; set; }
        public Transform Output { get; set; }
        public Vector3 TargetPosition { get; set; }

        private void Start()
        {
            _energy = FindObjectOfType<EnergyGlobeControl>();
        }

        // Update is called once per frame
        private void Update()
        {
            switch (Spell.SpellType)
            {
                case SpellType.UniqueEffect:
                    if (_isCasting && Vector3.Distance(_initialPosition, transform.position) >= Spell.SpellRange)
                    {
                        ProjectileImpact();
                    }
                    break;
                case SpellType.ContactEffect:
                    if (_isCasting)
                    {
                        _isCasting = false;
                        foreach (Transform child in transform)
                        {
                            child.rotation = Attacker.transform.rotation;
                        }
                        //transform.GetChild(0).rotation = Attacker.transform.rotation;
                        StartCoroutine(DealRadiusDamage());
                    }
                    break;
                case SpellType.ZoneEffect:
                    if (_isCasting)
                    {
                        _isCasting = false;
                        StartCoroutine(DamageOverTimeInArea());
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void StartCast()
        {
            var projectileRigidbody = GetComponent<Rigidbody>();
            projectileRigidbody.isKinematic = speed == 0f;
            
            if (speed == 0f && Spell.SpellType == SpellType.ZoneEffect)
            {
                if (CompareTag("Player"))
                {
                    Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit);
                
                    if (Vector3.Distance(Attacker.transform.position , hit.point) <= Spell.SpellRange)
                    {
                        transform.position = hit.point;
                    }
                    else
                    {
                        var attackerTransform = Attacker.transform;
                        transform.position = attackerTransform.forward * Spell.SpellRange + attackerTransform.position;
                    }
                }
                else
                {
                    transform.position = TargetPosition;
                }
            }
            else if (speed > 0 && Spell.SpellType == SpellType.UniqueEffect)
            {
                _initialPosition = transform.position;
                projectileRigidbody.AddForce(GameObject.FindGameObjectWithTag("Player").transform.forward * speed);
            }
            else if (speed == 0f && Spell.SpellType == SpellType.ContactEffect && Spell.SpellEffect == SpellEffect.Heal)
            {
                transform.position = Attacker.transform.position;
            }

            if (Spell.SpellType == SpellType.ContactEffect) return;
            
            EndCast();
        }

        private void ProjectileImpact()
        {
            if (Spell.ParticleEffectImpact == null || !_isCasting) return;
            
            _isCasting = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            CleanChildren();
            UpdateParticle();
            StartCoroutine(DestroyProjectile());
        }

        private void CleanChildren()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void UpdateParticle()
        {
            var particle = Instantiate(Spell.ParticleEffectImpact, transform.position, Quaternion.identity);
            
            particle.transform.parent = transform;
            //particle.transform.localScale = Spell.ParticleSizeImpact;

            _destroyTimer = particle.GetComponent<ParticleSystem>().main.duration;
        }

        private IEnumerator DealRadiusDamage()
        {
            if (Spell.CanalisationTimer > 0f)
            {
                var count = 0;
                var timer = 0f;
                var spellFighter = Attacker.GetComponent<FighterSpell>();
                while ((Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.E) || count == 0) && Attacker.GetComponent<ActionScheduler>().CurrentAction == Attacker.GetComponent<FighterSpell>())
                {
                    timer += Time.deltaTime;
                    if (timer >= Spell.CanalisationTimer)
                    {
                        timer = 0f;
                        HitTargetInRadius();
                        _energy.UseEnergy(Spell.spellCost);
                    }
                    
                    yield return null;
                    
                    spellFighter.UpdatePlayerRotation();
                    UpdateSpellPosition();
                    count += 1;
                    
                    if (Attacker.CompareTag("Player") &&
                        !FindObjectOfType<EnergyGlobeControl>().HasEnoughEnergy(Spell.spellCost)) break;
                }
            }
            else
            {
                HitTargetInRadius();
                yield return new WaitForSeconds(.75f);
            }
            
            EndCast();
            Destroy(gameObject);
        }

        private void UpdateSpellPosition()
        {
            var spellTransform = transform;
            
            spellTransform.GetChild(0).rotation = Attacker.transform.rotation;
            spellTransform.position = Spell.SpellEffect == SpellEffect.Heal ? Attacker.transform.position : Output.position;
        }

        private void HitTargetInRadius()
        {
            var colliders = Physics.OverlapSphere(Attacker.transform.position, Spell.SpellRange);
            foreach (var newTarget in colliders)
            {
                var targetHealth = newTarget.GetComponent<Health>();
                
                if (!Fighter.CanAttack(newTarget.gameObject) || !targetHealth.isActiveAndEnabled || !Attacker.GetIsInFieldOfView(targetHealth.transform, Spell.DamageRadius)) continue;

                if (Spell.SpellEffect == SpellEffect.Heal && Attacker.CompareTag(targetHealth.tag))
                {
                    targetHealth.GiveLife(Spell.SpellDamage);
                }
                else if (!Attacker.CompareTag(targetHealth.tag))
                {
                    targetHealth.TakeDamage(Spell.SpellDamage, false, Attacker);
                }
            }
        }

        private void DealDamage(Component target)
        {
            var colliderHealth = target.GetComponent<Health>();
            
            if (!Fighter.CanAttack(target.gameObject) || !colliderHealth.isActiveAndEnabled || _isCasting) return;

            if (Spell.SpellEffect == SpellEffect.Heal && Attacker.CompareTag(colliderHealth.tag))
            {
                colliderHealth.GiveLife(Spell.SpellDamage);
                colliderHealth.TakeDot(Spell, Attacker);
            }
            else if (!Attacker.CompareTag(colliderHealth.tag))
            {
                colliderHealth.TakeDamage(Spell.SpellDamage, false, Attacker);
                colliderHealth.TakeDot(Spell, Attacker);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name == "SafeZone" || Spell.SpellType != SpellType.UniqueEffect) return;
            
            ProjectileImpact();
            DealDamage(other);
        }

        private void EndCast()
        {
            Attacker.GetComponent<Animator>().ResetTrigger("stopCast");
            Attacker.GetComponent<Animator>().ResetTrigger("cast");
            Attacker.GetComponent<Animator>().SetTrigger("endCast");
        }

        private IEnumerator DamageOverTimeInArea()
        {
            for (var i = 0; i < Spell.DotCount; i++)
            {
                yield return new WaitForSeconds(Spell.DotTick); 
                
                var colliders = Physics.OverlapSphere(transform.position, Spell.SpellZoneArea / 2);
                foreach (var newTarget in colliders)
                {
                    var targetHealth = newTarget.GetComponent<Health>();
                    
                    if (!Fighter.CanAttack(newTarget.gameObject) || !targetHealth.isActiveAndEnabled) continue;

                    if (Spell.SpellEffect == SpellEffect.Heal && Attacker.CompareTag(targetHealth.tag))
                    {
                        targetHealth.GiveLife(Spell.DotDamage);
                    }
                    else if (!Attacker.CompareTag(targetHealth.tag))
                    {
                        targetHealth.TakeDamage(Spell.DotDamage, false, Attacker);
                    }
                }
            }
            
            Destroy(gameObject);
        }

        private IEnumerator DestroyProjectile()
        {
            yield return new WaitForSeconds(_destroyTimer);
            
            Destroy(gameObject);
        }
    }
}
