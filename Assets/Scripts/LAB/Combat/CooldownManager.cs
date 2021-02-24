using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class CooldownManager : MonoBehaviour
    {
        public static CooldownManager instance;
        
        private readonly List<Spell> _spellsOnCooldown = new List<Spell>();

        // Update is called once per frame
        private void Update()
        {
            foreach (var spell in _spellsOnCooldown.ToArray())
            {
                spell.CurrentCooldown = Math.Max(spell.CurrentCooldown - Time.deltaTime, 0);

                if (spell.CurrentCooldown > 0) continue;
                
                _spellsOnCooldown.Remove(spell);
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this);
            }
            
            DontDestroyOnLoad(this);
        }

        public void StartCooldown(Spell spell)
        {
            if (_spellsOnCooldown.Contains(spell)) return;

            spell.ResetCooldown();
            _spellsOnCooldown.Add(spell);
        }
    }
}
