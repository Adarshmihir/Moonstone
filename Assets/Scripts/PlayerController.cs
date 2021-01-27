using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerController : MonoBehaviour {
    public LayerMask movementMask;

    public Interactable focus;

    private Camera cam;
    private PlayerMotor motor;

    // Start is called before the first frame update
    private void Start() {
        cam = Camera.main;

        motor = GetComponent<PlayerMotor>();
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            // We create a ray
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If the ray hits
            if (Physics.Raycast(ray, out hit, 100, movementMask)) {
                motor.MoveToPoint(hit.point); // Move to where we hit

                RemoveFocus();
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            // We create a ray
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If the ray hits
            if (Physics.Raycast(ray, out hit, 100)) {
                // Check if we hit an interactable
                var interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null) SetFocus(interactable);
            }
        }
    }

    private void SetFocus(Interactable newFocus) {
        if (newFocus != focus) {
            if (focus != null)
                focus.OnDefocused();

            focus = newFocus;
            motor.FollowTarget(newFocus);
        }

        newFocus.OnFocused(transform);
    }

    private void RemoveFocus() {
        if (focus != null)
            focus.OnDefocused();
        focus = null;
        motor.StopFollowingTarget();
    }
}