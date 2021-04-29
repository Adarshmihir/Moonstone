using Control;
using Core;
using Movement;
using ResourcesHealth;
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

        [SerializeField] public RawImage weaponSlot;
        [SerializeField] public RawImage armorSlot;
        [SerializeField] public RawImage petSlot;

        private Spell _spellToCast;
        private CastSource _castSource;
        private Fighter _fighter;
        private Mover _mover;
        private AIController _aiController;

        public Spell WeaponSpell => weaponSpell;
        public Spell ArmorSpell => armorSpell;
        public Spell PetSpell => petSpell;

        //private Health Target { get; set; }

        private void Start()
        {
            /*_fighter = GetComponent<Fighter>();
            _mover = GetComponent<Mover>();
            
            UpdateSpell(temp, CastSource.Weapon);
            UpdateSpell(temp, CastSource.Armor);*/
        }

        public void InitializeFighterSpell()
        {
            _fighter = GetComponent<Fighter>();
            _mover = GetComponent<Mover>();
            _aiController = GetComponent<AIController>();

            if (!CompareTag("Player")) return;
            
            weaponSlot = GameManager.Instance.WeaponSlot;
            armorSlot = GameManager.Instance.ArmorSlot;
            petSlot = GameManager.Instance.PetSlot;
        }
        
        // Update is called once per frame
        private void Update()
        {
            //if (/*Target == null || Target.IsDead  || */_spellToCast.IsSpellOnCooldown()) return;

            // Check if target is not too far
            /*if (!_fighter.GetIsInRange(Target.transform.position, _spellToCast.SpellRange))
            {
                // Move towards the target until it is close enough
                _mover.MoveTo(Target.transform.position);
            }
            else
            {
                // Cancel movement action and start attack
                _mover.Cancel();
                CastBehaviour();
            }*/
        }

        public void UpdateSpell(Spell newSpell, CastSource castSource)
        {
            var slot = weaponSlot;
            switch (castSource)
            {
                case CastSource.Weapon:
                    weaponSpell = newSpell;
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
            UpdateSpellSlotUI(slot, newSpell != null ? newSpell.SpellIcon : null);
        }

        private static void UpdateSpellSlotUI(RawImage rawImage, Texture newSprite)
        {
            rawImage.texture = newSprite;
            rawImage.gameObject.SetActive(newSprite != null);
        }

        public void Cast(/*GameObject combatTarget, */CastSource castSource)
        {
            var energyPlayer = FindObjectOfType<EnergyGlobeControl>();

            // Start attack action
            GetComponent<ActionScheduler>().StartAction(this);

            InitSpellToCast(castSource);

            if (_spellToCast == null)
            {
                GameManager.Instance.FeedbackMessage.SetMessage("Vous n'avez pas de sort lié à cette touche");
                return;
            }

            if (_spellToCast.IsSpellOnCooldown()) return;

            if (CompareTag("Player") && !energyPlayer.HasEnoughEnergy(_spellToCast.spellCost))
            {
                return;
            }

            // Define target
            //Target = combatTarget.GetComponent<Health>();
            _castSource = castSource;
            
            CastBehaviour();

        }

        private void CastBehaviour()
        {
            UpdatePlayerRotation();
            
            CastAnimation();

            if (CompareTag("Player"))
            {
                _spellToCast.PutOnCooldown(_castSource);
            }
        }

        public void UpdatePlayerRotation()
        {
            if (CompareTag("Player"))
            {
                var hasHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit);

                if (!hasHit || Vector3.Distance(hit.point, transform.position) <= 1f) return;

                var playerPosition = transform.position;
                var finalPosition = new Vector3(hit.point.x, playerPosition.y, hit.point.z);
            
                // Rotate the character in direction of the target
                transform.LookAt(finalPosition);
            }
            else
            {
                transform.LookAt(GameManager.Instance.player.transform.position);
            }
        }

        private void InitSpellToCast(CastSource castSource)
        {
            switch (castSource)
            {
                case CastSource.Weapon:
                    _spellToCast = weaponSpell;
                    break;
                case CastSource.Armor:
                    _spellToCast = armorSpell;
                    break;
                case CastSource.Pet:
                    _spellToCast = petSpell;
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
            GetComponent<Animator>().ResetTrigger("endCast");
            GetComponent<Animator>().ResetTrigger("stopCast");
            GetComponent<Animator>().SetTrigger("cast");
        }

        // Animation event
        public void Shoot()
        {
            //_spellToCast.Launch(rightHandTransform, Target, _fighter);
            _spellToCast.Launch(rightHandTransform, _fighter, GameManager.Instance.player.transform.position);
            FindObjectOfType<EnergyGlobeControl>().UseEnergy(_spellToCast.spellCost);
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
            //Target = null;
        }
        
        private void StopCast()
        {
            // Stop cast animation
            _fighter.ChangeWeaponVisibility(true);
            GetComponent<Animator>().ResetTrigger("endCast");
            GetComponent<Animator>().ResetTrigger("cast");
        }
    }
}
