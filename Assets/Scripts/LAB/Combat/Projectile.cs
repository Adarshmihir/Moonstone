using System;
using System.Collections;
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
        
        public Spell Spell { get; set; }
        //public Health Target { get; set; }
        public Fighter Attacker { get; set; }

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
                        transform.GetChild(0).rotation = Attacker.transform.rotation;
                        StartCoroutine(DealRadiusDamage());
                    }
                    break;
                case SpellType.ZoneEffect:
                    if (_isCasting)
                    {
                        _isCasting = false;
                        //Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit);
                        //transform.LookAt(hit.point);
                        //transform.rotation = Attacker.transform.rotation;
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
                _initialPosition = transform.position;
                projectileRigidbody.AddForce(GameObject.FindGameObjectWithTag("Player").transform.forward * speed);
            }
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
            particle.transform.localScale = Spell.ParticleSizeImpact;

            _destroyTimer = particle.GetComponent<ParticleSystem>().main.duration;
        }

        private IEnumerator DealRadiusDamage()
        {
            var colliders = Physics.OverlapSphere(Attacker.transform.position, Spell.SpellRange);
            foreach (var newTarget in colliders)
            {
                var targetHealth = newTarget.GetComponent<Health>();
                    
                if (targetHealth == null || Attacker.CompareTag(targetHealth.tag) || targetHealth.IsDead || !Attacker.GetIsInFieldOfView(targetHealth.transform, Spell.DamageRadius)) continue;

                targetHealth.TakeDamage(Spell.SpellDamage, false, Attacker);
            }
            
            yield return new WaitForSeconds(.75f);
            Destroy(gameObject);
        }

        private void DealDamage(Component target)
        {
            var colliderHealth = target.GetComponent<Health>();
            
            if (colliderHealth == null || Attacker.CompareTag(colliderHealth.tag) || colliderHealth.IsDead || _isCasting) return;
            
            colliderHealth.TakeDamage(Spell.SpellDamage, false, Attacker);
            colliderHealth.TakeDot(Spell, Attacker);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name == "SafeZone" || Spell.SpellType != SpellType.UniqueEffect) return;
            
            ProjectileImpact();
            DealDamage(other);
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
                    
                    if (targetHealth == null || Attacker.CompareTag(targetHealth.tag) || targetHealth.IsDead) continue;
                    
                    targetHealth.TakeDamage(Spell.DotDamage, false, Attacker);
                }
            }
            
            Destroy(gameObject);
        }

        private IEnumerator DestroyProjectile()
        {
            yield return new WaitForSeconds(_destroyTimer);
            
            Destroy(gameObject);
        }

        /*private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() == null) return;
            
            Target.TakeDamage(Spell.SpellDamage, false, Attacker);

            if (Spell.DotCount > 0 && !Target.Dots.Contains(Spell.name))
            {
                Target.Dots.Add(Spell.name);
                StartCoroutine(StartDot());
            }
            else
            {
                StartCoroutine(StartGameObjectDestroy());
            }
        }*/

        /*private IEnumerator StartDot()
        {
            for (var i = 0; i < Spell.DotCount; i++)
            {
                yield return new WaitForSeconds(Spell.DotTick);
                
                Target.TakeDamage(Spell.DotDamage, false, Attacker);
                transform.GetChild(0).localScale *= 0.85f;
            }

            Target.Dots.Remove(Spell.name);
            Destroy(gameObject);
        }*/

        /*private IEnumerator StartGameObjectDestroy()
        {
            var particle = transform.GetChild(0);
            var originalScale = particle.localScale;
            
            while (particle.localScale.magnitude > originalScale.magnitude * 0.25f)
            {
                particle.localScale *= 0.9f;
                yield return new WaitForSeconds(0.2f);
            }
            
            Destroy(gameObject);
        }*/
    }
}
