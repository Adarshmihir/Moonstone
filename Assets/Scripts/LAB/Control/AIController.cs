using Combat;
using Core;
using Movement;
using Resources;
using UnityEngine;

namespace Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private Patroller patroller;
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float maxDistance = 15f;
        [SerializeField] private float aggroTimer = 1f;
        [SerializeField] private float breakTimer = 5f;

        private Fighter fighter;
        private Health health;
        private GameObject player;
        private Mover mover;
        private ActionScheduler actionScheduler;

        private Vector3 initialPosition;
        private float timeSinceLastAggro;
        private float timeSinceLastBreak;
        private int currentWaypoint;
        private const float WaypointDistTolerance = 1f;

        // Start is called before the first frame update
        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();

            initialPosition = transform.position;
            timeSinceLastAggro = aggroTimer + 1;
        }
        
        // Update is called once per frame
        private void Update()
        {
            if (health.IsDead || player == null) return;
            
            if (DistanceToPlayer() && Fighter.CanAttack(player) && DistanceToInitialPosition() && timeSinceLastAggro > aggroTimer)
            {
                fighter.Attack(player);
            }
            else
            {
                timeSinceLastAggro += Time.deltaTime;
                if (ReferenceEquals(actionScheduler.CurrentAction, fighter))
                {
                    timeSinceLastAggro = 0;
                }
                
                PatrolBehaviour();
            }
            
            timeSinceLastBreak += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            if (patroller != null)
            {
                if (DistanceToWaypoint())
                {
                    currentWaypoint = patroller.GetNextWaypoint(currentWaypoint);
                    initialPosition = patroller.GetWaypoint(currentWaypoint);
                    timeSinceLastBreak = 0;
                }
            }

            if (timeSinceLastBreak > breakTimer)
            {
                mover.StartMoveAction(initialPosition);
            }
        }

        private bool DistanceToWaypoint()
        {
            return Vector3.Distance(transform.position, patroller.GetWaypoint(currentWaypoint)) < WaypointDistTolerance;
        }

        private bool DistanceToPlayer()
        {
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
        }

        private bool DistanceToInitialPosition()
        {
            return Vector3.Distance(initialPosition, transform.position) < maxDistance;
        }
    }
}
