using System.Collections;
using Resources;
using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;

        public Spell Spell { get; set; }
        public Health Target { get; set; }
        public Fighter Attacker { get; set; }
        
        // Update is called once per frame
        private void Update()
        {
            if (Target == null) return;
            
            transform.LookAt(GetHitLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private Vector3 GetHitLocation()
        {
            var targetCaps = Target.GetComponent<CapsuleCollider>();
            var position = Target.transform.position;
            return targetCaps == null ? position : position + Vector3.up * targetCaps.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() == null) return;
            
            Target.TakeDamage(Spell.SpellDamage, false, Attacker);
            StartCoroutine(StartGameObjectDestroy());
        }

        private IEnumerator StartGameObjectDestroy()
        {
            var particle = transform.GetChild(0);
            var originalScale = particle.localScale;
            
            while (particle.localScale.magnitude > originalScale.magnitude * 0.25f)
            {
                particle.localScale *= 0.9f;
                yield return new WaitForSeconds(0.2f);
            }
            
            Destroy(gameObject);
        }
    }
}
