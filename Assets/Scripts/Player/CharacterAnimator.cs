using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour {
    private const float locomotionAnimationSmoothTime = .01f;

    private NavMeshAgent agent;
    private Animator animator;

    // Start is called before the first frame update
    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update() {
        var speedPercent = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
    }
}