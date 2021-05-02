using System.Collections;
using Combat;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Control
{
    public class AISpellController : MonoBehaviour
    {
        [SerializeField] private float restTimer = 1.5f;
        [SerializeField] private float timeBetweenSpell = 25f;
        [SerializeField] [Range(0, 100)] private int chanceToCastSpell = 35;
        
        private FighterSpell _fighterSpell;
        private Fighter _fighter;
        private AIController _aiController;
        
        private bool _canCast;

        private void Start()
        {
            _fighterSpell = GetComponent<FighterSpell>();
            _fighter = GetComponent<Fighter>();
            _aiController = GetComponent<AIController>();

            _fighterSpell.InitializeFighterSpell();
            StartCoroutine(UpdateTimeBetweenSpell());
        }

        // Update is called once per frame
        private void Update()
        {
            if (_fighter.Target == null || !_canCast) return;
            
            GetSpell();
        }

        private void GetSpell()
        {
            if (_fighterSpell.WeaponSpell != null && Random.Range(0, 100) < chanceToCastSpell)
            {
                CastSpell(CastSource.Weapon, _fighterSpell.WeaponSpell);
            }
            
            if (_fighterSpell.ArmorSpell != null && Random.Range(0, 100) < chanceToCastSpell)
            {
                CastSpell(CastSource.Armor, _fighterSpell.ArmorSpell);
            }
            
            if (_fighterSpell.PetSpell != null && Random.Range(0, 100) < chanceToCastSpell)
            {
                CastSpell(CastSource.Pet, _fighterSpell.PetSpell);
            }
        }

        private void CastSpell(CastSource castSource, Spell spell)
        {
            if (spell.IsSpellOnCooldown()) return;

            StartCoroutine(UpdateTimeBetweenSpell());
            _aiController.UpdateRestTimer(restTimer);
            _fighterSpell.Cast(castSource);
        }

        private IEnumerator UpdateTimeBetweenSpell()
        {
            _canCast = false;
            
            yield return new WaitForSeconds(timeBetweenSpell);

            _canCast = true;
        }
    }
}
