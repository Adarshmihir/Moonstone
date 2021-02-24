using Combat;
using Movement;
using Resources;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Canvas statsCanvas;
        public Interactable focus;

        private Health _health;
        private Fighter _fighter;
        private FighterSpell _fighterSpell;

        // Start is called before the first frame update
        private void Start()
        {
            _health = GetComponent<Health>();
            _fighter = GetComponent<Fighter>();
            _fighterSpell = GetComponent<FighterSpell>();
        }
        
        // Update is called once per frame
        private void Update()
        {
            if (_health.IsDead || EventSystem.current.IsPointerOverGameObject()) return;

            if (Input.GetKeyDown(KeyCode.A))
            {
                Canvas statsCanvas = GameManager.Instance.uiManager.StatsCanvasGO.GetComponent<Canvas>();
                statsCanvas.gameObject.SetActive(!statsCanvas.gameObject.activeSelf);
            }
            
            if (InteractWithCombat(false)) return;
            if (InteractWithCombat(true)) return;
            if (InteractWithObjects()) return;
            InteractWithMovement();
        }

        private bool InteractWithCombat(bool autoAttack)
        {
            var hits = Physics.RaycastAll(GetMouseRay());
            foreach (var hit in hits)
            {
                var target = hit.transform.GetComponent<CombatTarget>();
                if (target == null || !Fighter.CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButtonDown(0) && autoAttack)
                {
                    _fighter.Attack(target.gameObject);
                }
                else if (Input.GetMouseButtonDown(1) && !autoAttack)
                {
                    // TODO : Get Spell on Weapon
                    _fighterSpell.Cast(target.gameObject, CastSource.Weapon);
                    
                }
                return true;
            }
            return false;
        }

        private bool InteractWithObjects()
        {
            // If the ray hits
            Physics.Raycast(GetMouseRay(), out var hit, 100);
            // Check if we hit an interactable
            if (hit.collider == null) return false;
            
            var interactable = hit.collider.GetComponent<Interactable>();
            if (interactable == null || !Input.GetMouseButtonDown(1)) return false;
            
            SetFocus(interactable);
            return true;
        }

        private bool InteractWithMovement()
        {
            var hasHit = Physics.Raycast(GetMouseRay(), out var hit);
            if (!hasHit || !DistanceToNewPosition(hit.point)) return false;
            
            if (Input.GetMouseButton(0))
            {
                GetComponent<Mover>().StartMoveAction(hit.point);
            }
            return true;
        }
        
        private bool DistanceToNewPosition(Vector3 position)
        {
            return Vector3.Distance(position, transform.position) > 1f;
        }

        private static Ray GetMouseRay()
        {
            return !(Camera.main is null) ? Camera.main.ScreenPointToRay(Input.mousePosition) : default;
        }
        
        private void SetFocus(Interactable newFocus)
        {
            if (newFocus != focus)
            {
                if (focus != null)
                {
                    focus.OnDefocused();
                }
                focus = newFocus;
            }
            newFocus.OnFocused(transform);
        }
    }
}
