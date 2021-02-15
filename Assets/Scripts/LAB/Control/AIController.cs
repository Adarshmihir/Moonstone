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

        private Fighter _fighter;
        private Health _health;
        private GameObject _player;
        private Mover _mover;
        private ActionScheduler _actionScheduler;

        private Vector3 _initialPosition;
        private float _timeSinceLastAggro;
        private float _timeSinceLastBreak;
        private int _currentWaypoint;
        private const float WaypointDistTolerance = 1f;

        // Start is called before the first frame update
        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _actionScheduler = GetComponent<ActionScheduler>();

            _initialPosition = transform.position;
            _timeSinceLastAggro = aggroTimer + 1;
        }
        
        // Update is called once per frame
        private void Update()
        {
            if (_health.IsDead || _player == null) return;
            
            if (DistanceToPlayer() && Fighter.CanAttack(_player) && DistanceToInitialPosition() && _timeSinceLastAggro > aggroTimer)
            {
                _fighter.Attack(_player);
            }
            else
            {
                _timeSinceLastAggro += Time.deltaTime;
                if (ReferenceEquals(_actionScheduler.CurrentAction, _fighter))
                {
                    _timeSinceLastAggro = 0;
                }
                
                PatrolBehaviour();
            }
            
            _timeSinceLastBreak += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            if (patroller != null)
            {
                if (Vector3.Distance(transform.position, patroller.GetWaypoint(_currentWaypoint)) < WaypointDistTolerance)
                {
                    _currentWaypoint = patroller.GetNextWaypoint(_currentWaypoint);
                    _initialPosition = patroller.GetWaypoint(_currentWaypoint);
                    _timeSinceLastBreak = 0;
                }
            }

            if (_timeSinceLastBreak > breakTimer)
            {
                _mover.StartMoveAction(_initialPosition);
            }
        }

        private bool DistanceToPlayer()
        {
            return Vector3.Distance(_player.transform.position, transform.position) < chaseDistance;
        }

        private bool DistanceToInitialPosition()
        {
            return Vector3.Distance(_initialPosition, transform.position) < maxDistance;
        }
    }
}
