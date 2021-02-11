using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace UI.DamageText
{
    public enum DamageType
    {
        Normal,
        Critical,
        Heal
    }
    
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private Text damageText;

        [SerializeField] private int damageTextSize = 14;
        [SerializeField] private int criticalTextSize = 20;
        [SerializeField] private int healTextSize = 14;

        [SerializeField] private Color damageTextColor = Color.red;
        [SerializeField] private Color criticalTextColor = Color.yellow;
        [SerializeField] private Color healTextColor = Color.green;

        private void Update()
        {
            if (Camera.main == null) return;
            
            var rotation = Camera.main.transform.rotation;
            transform.LookAt(transform.position + rotation *- Vector3.back, rotation *- Vector3.down);
        }

        public void SetDamageText(float damage, DamageType damageType)
        {
            var damagePrefix = "";
            
            switch (damageType)
            {
                case DamageType.Normal:
                    damageText.color = damageTextColor;
                    damageText.fontSize = damageTextSize;
                    break;
                
                case DamageType.Critical:
                    damageText.color = criticalTextColor;
                    damageText.fontStyle = FontStyle.Bold;
                    damageText.fontSize = criticalTextSize;
                    break;
                
                case DamageType.Heal:
                    damageText.color = healTextColor;
                    damageText.fontSize = healTextSize;
                    damagePrefix = "+ ";
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            damageText.text = damagePrefix + damage.ToString(CultureInfo.InvariantCulture);
        }
    }
}
