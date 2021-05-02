using System.Collections;
using UnityEngine;

namespace UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageTextPrefab;
        [SerializeField] private float textDuration = 2f;

        public void Spawn(float damageAmount, DamageType damageType)
        {
            var instance = Instantiate(damageTextPrefab, transform);
            instance.SetDamageText(damageAmount, damageType);
            
            StartCoroutine(DestroyInstance(instance));
        }

        private IEnumerator DestroyInstance(Component instance)
        {
            yield return new WaitForSeconds(textDuration);
            
            Destroy(instance.gameObject);
        }
    }
}
