using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : MonoBehaviour
{
    private List<Transform> _waypoints;
    
    // Start is called before the first frame update
    private void Start()
    {
        _waypoints = new List<Transform>();
        for (var i = 0; i < transform.childCount; i++)
        {
            _waypoints.Add(transform.GetChild(i));
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public Vector3 GetWaypoint(int currentWaypoint)
    {
        return _waypoints[currentWaypoint].position;
    }

    public int GetNextWaypoint(int currentWaypoint)
    {
        return (currentWaypoint + 1) % transform.childCount;
    }
}
