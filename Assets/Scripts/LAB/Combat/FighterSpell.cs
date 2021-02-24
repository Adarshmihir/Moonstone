using System;
using Core;
using Resources;
using UnityEngine;

namespace Combat
{
    public enum CastSource
    {
        Weapon,
        Armor,
        Pet
    }
    
    public class FighterSpell : MonoBehaviour, IAction
    {
        [SerializeField] private Spell weaponSpell;
        [SerializeField] private Spell armorSpell;
        [SerializeField] private Spell petSpell;
        
        [SerializeField] private Transform rightHandTransform;

        private Spell _spellToCast;
        private Fighter _fighter;

        private Health Target { get; set; }

        private void Start()
        {
            _fighter = GetComponent<Fighter>();
        }

        public void Cast(GameObject combatTarget, CastSource castSource)
        {
            InitSpellToCast(castSource);
            
            if (_spellToCast.IsSpellOnCooldown()) return;
            
            CastBehaviour(combatTarget);
        }

        private void CastBehaviour(GameObject combatTarget)
        {
            // Start attack action
            GetComponent<ActionScheduler>().StartAction(this);

            // Define target
            Target = combatTarget.GetComponent<Health>();
            
            // Rotate the character in direction of the target
            transform.LookAt(Target.transform);
            
            CastAnimation();
            _spellToCast.PutOnCooldown();
        }

        private void InitSpellToCast(CastSource castSource)
        {
            switch (castSource)
            {
                case CastSource.Weapon:
                    _spellToCast = weaponSpell;
                    break;
                case CastSource.Armor:
                    // TODO : Cast armor spell
                    break;
                case CastSource.Pet:
                    // TODO : Cast pet spell
                    break;
                default:
                    _spellToCast = weaponSpell;
                    break;
            }
        }
        
        private void CastAnimation()
        {
            // Start cast
            GetComponent<Animator>().ResetTrigger("stopCast");
            GetComponent<Animator>().SetTrigger("cast");
        }

        // Animation event
        public void Shoot()
        {
            _spellToCast.Launch(rightHandTransform, Target);
        }

        // Animation event
        public void HideWeapon()
        {
            _fighter.ChangeWeaponVisibility(false);
        }

        // Animation event
        public void ShowWeapon()
        {
            _fighter.ChangeWeaponVisibility(true);
        }
        
        public void Cancel()
        {
            // Stop cast action
            StopCast();
            // Remove target
            Target = null;
        }
        
        private void StopCast()
        {
            // Stop cast animation
            GetComponent<Animator>().ResetTrigger("cast");
            GetComponent<Animator>().SetTrigger("stopCast");
        }
    }
}
