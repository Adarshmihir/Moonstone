using System.Collections.Generic;
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

        private readonly Dictionary<Fighter, float> _aggroRate = new Dictionary<Fighter, float>();
        private Vector3 _initialPosition;
        private float _timeSinceLastAggro;
        private float _timeSinceLastBreak;
        private int _currentWaypoint;
        private bool _isAttacked;
        private float _timeAttacked;
        private const float WaypointDistTolerance = 1f;

        public bool IsGoingHome { get; private set; }

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

            if (DistanceToPlayer() && Fighter.CanAttack(_player) && _timeSinceLastAggro > aggroTimer && !IsGoingHome || _isAttacked)
            {
                _fighter.Attack(_player);

                if (!DistanceToInitialPosition())
                {
                    _isAttacked = false;
                    IsGoingHome = true;
                }
            }
            else
            {
                if (_aggroRate.Count > 0)
                {
                    _aggroRate.Clear();
                }
                
                if (IsGoingHome)
                {
                    _health.RegenLife();
                }
                
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
                if (DistanceToWaypoint())
                {
                    _currentWaypoint = patroller.GetNextWaypoint(_currentWaypoint);
                    _initialPosition = patroller.GetWaypoint(_currentWaypoint);
                    _timeSinceLastBreak = 0;
                    IsGoingHome = false;
                }
            }

            if (_timeSinceLastBreak > breakTimer)
            {
                _mover.StartMoveAction(_initialPosition);
            }
        }

        public void ConfigureTarget(Fighter attacker, float damage)
        {
            UpdateAggroDic(attacker, damage);
            
            /*var aggroDamage = _aggroRate[attacker];
            var aggroTarget = attacker;
                
            foreach (var aggro in _aggroRate.Where(aggro => aggro.Value > aggroDamage))
            {
                aggroDamage = aggro.Value;
                aggroTarget = aggro.Key;
            }*/

            //_fighter.Attack(aggroTarget.gameObject);

            _isAttacked = true;
            
            // TODO : Cast call zone and dic
            
            //_fighter.Attack(_player);
            
            // TODO : Create call zone to allies mate around
        }

        private void UpdateAggroDic(Fighter attacker, float damage)
        {
            if (!_aggroRate.ContainsKey(attacker))
            {
                _aggroRate.Add(attacker, damage);
            }
            else
            {
                _aggroRate[attacker] += damage;
            }
        }

        private bool DistanceToWaypoint()
        {
            return Vector3.Distance(transform.position, patroller.GetWaypoint(_currentWaypoint)) < WaypointDistTolerance;
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
