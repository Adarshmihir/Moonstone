using System;
using Movement;
using UnityEngine;

public class Zone : MonoBehaviour {
    public float ZoneRadius = 3f;
    private Transform zoneTransform;
    public string name;
    private bool isInside;
    private Transform player;
    private SphereCollider collider;

    private void Start()
    {
        collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = ZoneRadius;
        collider.isTrigger = true;
        zoneTransform = GetComponent<Transform>();
        player = GameObject.FindWithTag("Player").transform; //WORKAROUND
    }


    private void OnDrawGizmosSelected() {
        if (zoneTransform == null)
            zoneTransform = transform;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(zoneTransform.position, ZoneRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        UseTriggerZone(other);
    }

    protected virtual void UseTriggerZone(Collider col)
    {
        Debug.Log("Entering in " + name + " zone !");
    }

}