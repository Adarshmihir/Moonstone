using Resources;
using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;

        public Health Target { get; set; }
        
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
    }
}
