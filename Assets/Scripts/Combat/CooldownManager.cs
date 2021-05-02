using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class CooldownManager : MonoBehaviour
    {
        [SerializeField] private List<Image> spellsIcons = new List<Image>();
        
        public static CooldownManager instance;
        
        private readonly List<Tuple<Spell, CastSource>> _spellsOnCooldown = new List<Tuple<Spell, CastSource>>();

        private void Start()
        {
            foreach (var icon in spellsIcons)
            {
                icon.fillAmount = 0;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            foreach (var spell in _spellsOnCooldown.ToArray())
            {
                spell.Item1.CurrentCooldown = Math.Max(spell.Item1.CurrentCooldown - Time.deltaTime, 0);

                if (spell.Item1.CurrentCooldown > 0) continue;
                
                _spellsOnCooldown.Remove(spell);
            }
            
            UpdateCooldownUI();
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

        private void UpdateCooldownUI()
        {
            if (_spellsOnCooldown.Count == 0) return;

            foreach (var (item1, item2) in _spellsOnCooldown)
            {
                switch (item2)
                {
                    case CastSource.Weapon:
                        spellsIcons[0].fillAmount = item1.CurrentCooldown / item1.Cooldown;
                        break;
                    case CastSource.Armor:
                        spellsIcons[1].fillAmount = item1.CurrentCooldown / item1.Cooldown;
                        break;
                    case CastSource.Pet:
                        spellsIcons[2].fillAmount = item1.CurrentCooldown / item1.Cooldown;
                        break;
                    default:
                        spellsIcons[0].fillAmount = item1.CurrentCooldown / item1.Cooldown;
                        break;
                }
            }
        }

        public void StartCooldown(Spell spell, CastSource castSource)
        {
            var tuple = Tuple.Create(spell, castSource);
            if (_spellsOnCooldown.Contains(tuple)) return;

            spell.ResetCooldown();
            _spellsOnCooldown.Add(tuple);
        }
    }
}
