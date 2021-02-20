using System.Collections.Generic;
using UnityEngine;

public class Patroller : MonoBehaviour
{
    private readonly List<Transform> _waypoints = new List<Transform>();
    
    // Start is called before the first frame update
    private void Start()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            _waypoints.Add(transform.GetChild(i));
        }
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
