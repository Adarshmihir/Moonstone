using System.Collections.Generic;
using System.Linq;
using Combat;
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
        [SerializeField] private float breakTimer = 5f;
        [SerializeField] private float helpRadius = 5f;

        private Fighter _fighter;
        private Health _health;
        private GameObject _target;
        private Mover _mover;

        private readonly Dictionary<Fighter, float> _aggroRate = new Dictionary<Fighter, float>();
        private Vector3 _initialPosition;
        private float _timeSinceLastBreak;
        private int _currentWaypoint;
        private bool _isAttacked;
        private float _timeAttacked;
        private const float WaypointDistTolerance = 1f;

        public bool IsGoingHome { get; private set; }

        // Start is called before the first frame update
        private void Start()
        {
            _target = GameObject.FindWithTag("Player");
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();

            _initialPosition = transform.position;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_health.IsDead || _target == null) return;

            if (DistanceToPlayer() && Fighter.CanAttack(_target) && !IsGoingHome || _isAttacked)
            {
                AttackBehaviour();
            }
            else
            {
                PreparePatrolBehaviour();
                PatrolBehaviour();
            }

            _timeSinceLastBreak += Time.deltaTime;
        }

        private void PreparePatrolBehaviour()
        {
            if (_aggroRate.Count > 0)
            {
                _aggroRate.Clear();
            }

            if (IsGoingHome)
            {
                _health.RegenLife();
            }
        }

        private void AttackBehaviour()
        {
            GetMateAround();

            _fighter.Attack(_target);

            if (DistanceToInitialPosition()) return;

            _isAttacked = false;
            IsGoingHome = true;
        }

        private void PatrolBehaviour()
        {
            if (patroller != null)
            {
                if (DistanceToWaypoint())
                {
                    IsGoingHome = false;
                    _timeSinceLastBreak = 0;
                    
                    _currentWaypoint = patroller.GetNextWaypoint(_currentWaypoint);
                    _initialPosition = patroller.GetWaypoint(_currentWaypoint);
                }
            }
            
            if (!(_timeSinceLastBreak > breakTimer) && !IsGoingHome) return;
            
            _mover.StartMoveAction(_initialPosition);
        }

        public void ConfigureTarget(Fighter attacker, float damage)
        {
            var newTarget = GetTargetFromDic(attacker, damage);

            if (_target == null || ReferenceEquals(_target.gameObject, newTarget.gameObject))
            {
                _target = newTarget.gameObject;
            }

            _isAttacked = true;
        }

        private void GetMateAround()
        {
            var colliders = Physics.OverlapSphere(transform.position, helpRadius);
            foreach (var allies in colliders)
            {
                var alliesController = allies.GetComponent<AIController>();
                if (alliesController == null) continue;

                alliesController.CallForHelp(_target);
            }
        }

        private void CallForHelp(GameObject target)
        {
            _isAttacked = true;
            _target = target;
        }

        private Fighter GetTargetFromDic(Fighter attacker, float damage)
        {
            UpdateAggroDic(attacker, damage);

            return _aggroRate.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
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
            return Vector3.Distance(_target.transform.position, transform.position) < chaseDistance;
        }

        private bool DistanceToInitialPosition()
        {
            return Vector3.Distance(_initialPosition, transform.position) < maxDistance;
        }

        //Forces the AI to initial position
        public void returnToInitialPosition()
        {
            //_isAttacked = false;
            //IsGoingHome = true;
        }
    }
}
