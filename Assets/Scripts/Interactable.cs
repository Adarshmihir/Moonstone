using UnityEngine;

public class Interactable : MonoBehaviour {
    public float radius = 3f;

    public Transform interactionTransform;

    private bool hasInteracted;

    private bool isFocused;
    private Transform player;

    private void Update() {
        if (isFocused && !hasInteracted) {
            var distance = Vector3.Distance(player.position, interactionTransform.position);
            if (distance <= radius) {
                Interact();
                hasInteracted = true;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        if (interactionTransform == null)
            interactionTransform = transform;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

    public virtual void Interact() {
        //This method is meant to be overwritten
        Debug.Log("Interacting with " + transform.name + " !");
    }

    public void OnFocused(Transform playerTransform) {
        isFocused = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDefocused() {
        isFocused = false;
        player = null;
        hasInteracted = false;
    }
}