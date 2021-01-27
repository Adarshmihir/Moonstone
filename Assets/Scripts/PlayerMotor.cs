using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] //Adds a NavMeshAgent wherever we use the PlayeMotor component
public class PlayerMotor : MonoBehaviour {
    private NavMeshAgent agent; // Reference to our agent
    private Transform target; // Target to follow

    // Start is called before the first frame update
    private void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update() {
        if (target != null) {
            agent.SetDestination(target.position);
            FaceTarget();
        }
    }

    public void MoveToPoint(Vector3 point) {
        agent.SetDestination(point);
    }

    public void FollowTarget(Interactable newTarget) {
        agent.stoppingDistance = newTarget.radius * .8f;
        agent.updateRotation = false;

        target = newTarget.interactionTransform;
    }

    public void StopFollowingTarget() {
        target = null;
        agent.updateRotation = true;
    }

    private void FaceTarget() {
        var direction = (target.position - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}