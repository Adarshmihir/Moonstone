using Core;
using Movement;
using Resources;
using UnityEngine;
using UnityEngine.UI;

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

        // TODO : Remove
        [SerializeField] private Spell temp;
        
        [SerializeField] private Transform rightHandTransform;

        [SerializeField] private RawImage weaponSlot;
        [SerializeField] private RawImage armorSlot;
        [SerializeField] private RawImage petSlot;

        private Spell _spellToCast;
        private CastSource _castSource;
        private Fighter _fighter;
        private Mover _mover;

        private Health Target { get; set; }

        private void Start()
        {
            _fighter = GetComponent<Fighter>();
            _mover = GetComponent<Mover>();
            
            UpdateSpell(temp, CastSource.Weapon);
            UpdateSpell(temp, CastSource.Armor);
        }
        
        // Update is called once per frame
        private void Update()
        {
            if (Target == null || Target.IsDead  || _spellToCast.IsSpellOnCooldown()) return;

            // Check if target is not too far
            if (!_fighter.GetIsInRange(Target.transform.position, _spellToCast.SpellRange))
            {
                // Move towards the target until it is close enough
                _mover.MoveTo(Target.transform.position);
            }
            else
            {
                // Cancel movement action and start attack
                _mover.Cancel();
                CastBehaviour();
            }
        }

        private void UpdateSpell(Spell newSpell, CastSource castSource)
        {
            var slot = weaponSlot;
            switch (castSource)
            {
                case CastSource.Weapon:
                    weaponSpell = newSpell;
                    UpdateSpellSlotUI(weaponSlot, newSpell.SpellIcon);
                    break;
                case CastSource.Armor:
                    armorSpell = newSpell;
                    slot = armorSlot;
                    break;
                case CastSource.Pet:
                    petSpell = newSpell;
                    slot = petSlot;
                    break;
                default:
                    weaponSpell = newSpell;
                    break;
            }
            UpdateSpellSlotUI(slot, newSpell.SpellIcon);
        }

        private static void UpdateSpellSlotUI(RawImage rawImage, Texture newSprite)
        {
            rawImage.texture = newSprite;
        }

        public void Cast(GameObject combatTarget, CastSource castSource)
        {
            // Start attack action
            GetComponent<ActionScheduler>().StartAction(this);
            
            InitSpellToCast(castSource);
            
            // Define target
            Target = combatTarget.GetComponent<Health>();
            _castSource = castSource;
        }

        private void CastBehaviour()
        {
            // Rotate the character in direction of the target
            transform.LookAt(Target.transform);
            
            CastAnimation();
            _spellToCast.PutOnCooldown(_castSource);
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
            if (!_spellToCast.IsAnimated) return;
            
            // Start cast
            GetComponent<Animator>().ResetTrigger("stopCast");
            GetComponent<Animator>().SetTrigger("cast");
        }

        // Animation event
        public void Shoot()
        {
            _spellToCast.Launch(rightHandTransform, Target, _fighter);
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
            _fighter.ChangeWeaponVisibility(true);
            GetComponent<Animator>().ResetTrigger("cast");
            GetComponent<Animator>().SetTrigger("stopCast");
        }
    }
}
