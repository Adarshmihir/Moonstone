using UnityEngine;

namespace Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private float _newAngle;
        private float _zoom = 1f;

        // Update is called once per frame
        private void Update() {
            _newAngle = Input.GetAxis("Horizontal") > 0 ? 1 : Input.GetAxis("Horizontal") < 0 ? -1 : 0;

            if (Input.GetAxis("Mouse ScrollWheel") > 0 && _zoom < 2)
            {
                _zoom += 0.1f;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && _zoom > 0.4)
            {
                _zoom -= 0.1f;
            }
        }
        
        private void LateUpdate()
        {
            if (target == null) return;

            var charTransform = transform;
            charTransform.position = target.position;
            charTransform.localScale = new Vector3(_zoom, _zoom, _zoom);
            
            transform.RotateAround(charTransform.position, Vector3.up, 60 * Time.deltaTime * _newAngle);
        }
    }
}
