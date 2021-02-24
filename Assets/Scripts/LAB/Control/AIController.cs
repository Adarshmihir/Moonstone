using Combat;
using Movement;
using Resources;
using UnityEngine;

namespace Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float maxDistance = 15f;
        private float m_MarginDistancefromInitialPosition = 1f;

        private Fighter _fighter;
        private Health _health;
        private GameObject _player;
        private Mover _mover;
        private bool b_returnToInitialPosition;
        private Vector3 _initialPosition;
            
        // Start is called before the first frame update
        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();

            _initialPosition = transform.position;
            b_returnToInitialPosition = false;
        }
        
        // Update is called once per frame
        private void Update()
        {
            if (_health.IsDead || _player == null) return;
            
            //If AI is not in safe zone
            if (!b_returnToInitialPosition)
            {
                if (DistanceToPlayer() && Fighter.CanAttack(_player) && DistanceToInitialPosition())
                {
                    _fighter.Attack(_player);
                }
                else
                {
                    _mover.StartMoveAction(_initialPosition);
                }
            }
            else
            {
                //AI comes back to original position
                _mover.StartMoveAction(_initialPosition);
                if (Vector3.Distance(transform.position, _initialPosition) <= m_MarginDistancefromInitialPosition)
                {
                    b_returnToInitialPosition = false;
                }
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
        
        //Forces the AI to initial position
        public void returnToInitialPosition()
        {
            b_returnToInitialPosition = true;
        }
    }
}
