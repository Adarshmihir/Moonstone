using System;
using Combat;
using Core;
using Resources;
using UnityEngine;

namespace Control
{
    public class AISpellController : MonoBehaviour
    {
        [SerializeField] private float restTimer = 1.5f;
        
        private FighterSpell _fighterSpell;
        private Fighter _fighter;
        private Health _health;
        private AIController _aiController;

        private bool temp;

        private void Start()
        {
            _fighterSpell = GetComponent<FighterSpell>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _aiController = GetComponent<AIController>();

            _fighterSpell.InitializeFighterSpell(null, null, null);
        }

        // Update is called once per frame
        private void Update()
        {
            if (_fighter.Target == null) return;
            
            GetSpell();
        }

        private void GetSpell()
        {
            if (_fighterSpell.WeaponSpell != null && !temp)
            {
                CastSpell(CastSource.Weapon, _fighterSpell.WeaponSpell);
            }
            
            /*if (_fighterSpell.ArmorSpell != null)
            {
                CastSpell(CastSource.Weapon, _fighterSpell.ArmorSpell);
            }
            
            if (_fighterSpell.PetSpell != null)
            {
                CastSpell(CastSource.Weapon, _fighterSpell.PetSpell);
            }*/
        }

        private void CastSpell(CastSource castSource, Spell spell)
        {
            if (spell.IsSpellOnCooldown()) return;

            switch (spell.SpellEffect)
            {
                case SpellEffect.Heal when _health.HealthPoints < _health.MaxHealthPoints * 0.8:
                case SpellEffect.Damage:
                    temp = true;
                    _aiController.UpdateRestTimer(restTimer);
                    _fighterSpell.Cast(castSource);
                    break;
            }
        }
    }
}
