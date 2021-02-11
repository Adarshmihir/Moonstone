using UnityEngine;

namespace Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private float newAngle;
        private float zoom = 1f;

        // Update is called once per frame
        private void Update() {
            newAngle = Input.GetAxis("Horizontal") > 0 ? 1 : Input.GetAxis("Horizontal") < 0 ? -1 : 0;

            if (Input.GetAxis("Mouse ScrollWheel") > 0 && zoom < 2)
            {
                zoom += 0.1f;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && zoom > 0.4)
            {
                zoom -= 0.1f;
            }
        }
        
        private void LateUpdate()
        {
            if (target == null) return;

            transform.position = target.position;
            transform.localScale = new Vector3(zoom, zoom, zoom);
            
            transform.RotateAround(transform.position, Vector3.up, 60 * Time.deltaTime * newAngle);
        }
    }
}
