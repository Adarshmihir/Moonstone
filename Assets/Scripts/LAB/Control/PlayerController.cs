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

        private Health health;
        private Mover mover;
        private Fighter fighter;

        // Start is called before the first frame update
        private void Start()
        {
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }
        
        // Update is called once per frame
        private void Update()
        {
            if (health.IsDead || EventSystem.current.IsPointerOverGameObject()) return;

            if (Input.GetKeyDown(KeyCode.A))
            {
                statsCanvas.gameObject.SetActive(!statsCanvas.gameObject.activeSelf);
            }
            
            if (InteractWithCombat()) return;
            if (InteractWithObjects()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            var hits = Physics.RaycastAll(GetMouseRay());
            foreach (var hit in hits)
            {
                var target = hit.transform.GetComponent<CombatTarget>();
                if (target == null || !Fighter.CanAttack(target.gameObject)) continue;

                // Input.GetMouseButton(0)
                if (Input.GetMouseButtonDown(0))
                {
                    fighter.Attack(target.gameObject);
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
                mover.StartMoveAction(hit.point);
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
