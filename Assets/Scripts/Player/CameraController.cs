using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;

    public Vector3 offset;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    public float pitch = 2f;

    public float yawSpeed = 100f;
    public float currentYaw;

    private float currentZoom = 10f;

    // Update is called once per frame
    private void Update() {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
    }
    
    private void LateUpdate() {
        var position = target.position;
        transform.position = position - offset * currentZoom;
        transform.LookAt(position + Vector3.up * pitch);

        transform.RotateAround(position, Vector3.up, currentYaw);
    }
}