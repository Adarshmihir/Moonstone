using System.Collections;
using Resources;
using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private float maxDistance = 15f;

        private Vector3 _initialPosition;
        private bool _casting = true;
        private float _destroyTimer;
        
        public Spell Spell { get; set; }
        //public Health Target { get; set; }
        public Fighter Attacker { get; set; }

        // Update is called once per frame
        private void Update()
        {
            if (!_casting || !(Vector3.Distance(_initialPosition, transform.position) >= maxDistance)) return;
            
            ProjectileImpact();
        }

        public void StartCast()
        {
            _initialPosition = transform.position;
            GetComponent<Rigidbody>().AddForce(GameObject.FindGameObjectWithTag("Player").transform.forward * speed);
        }

        private void ProjectileImpact()
        {
            if (Spell.ParticleEffectImpact == null || !_casting) return;
            
            _casting = false;
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

        private void DealDamage(Component target)
        {
            var colliderHealth = target.GetComponent<Health>();
            
            if (colliderHealth == null || _casting) return;
            
            colliderHealth.TakeDamage(Spell.SpellDamage, false, Attacker);
            colliderHealth.TakeDot(Spell, Attacker);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name == "SafeZone") return;
            
            ProjectileImpact();
            DealDamage(other);
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
