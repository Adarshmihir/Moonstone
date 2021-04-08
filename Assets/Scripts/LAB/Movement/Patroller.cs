﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patroller : MonoBehaviour
{
    private readonly List<Transform> _waypoints = new List<Transform>();
    private  List<GameObject> wayPointsList = new List<GameObject>();
    
    public float patrolRadius;
    // Start is called before the first frame update
    private void Start()
    {
        initStart();
    }

    private void initStart()
    {
        if (_waypoints.Count == 0)
        {
            initWaypoint();
        }
    } 

    public Vector3 GetWaypoint(int currentWaypoint)
    {

        initStart();

        return _waypoints[currentWaypoint].position;
    }

    public int GetNextWaypoint(int currentWaypoint)
    {
        return (currentWaypoint + 1) % _waypoints.Count;
    }

    public void initWaypoint() {
        int numberPoint = Random.Range(2, 5);
        GameObject g = new GameObject();
        g.transform.localPosition = this.transform.parent.position;
        g.transform.parent = this.transform;
        wayPointsList.Add(g);
        _waypoints.Add(g.transform);

        for (var i = 0; i < numberPoint - 1; i++)
        {
            g = new GameObject();
            g.transform.localPosition = GetRandomVector3Point();
            g.transform.parent = this.transform;
            wayPointsList.Add(g);
            _waypoints.Add(g.transform);
        }

        Debug.Log(transform.parent.name + " numberPoint = " + numberPoint + " size waypoints " + _waypoints.Count + " size wayList "  + this.wayPointsList.Count);
    }

    private Vector3 GetRandomVector3Point()
    {
        var randPos = new Vector3(0, 0, 0);

        NavMeshHit hit;
        var bIsPosValid = true;

        while (bIsPosValid)
        {
            randPos = Random.insideUnitSphere * patrolRadius;
            randPos += transform.position;

            bool test = NavMesh.SamplePosition(randPos, out hit, patrolRadius, 1);
            //Debug.Log(test);
            if (test) //only check walkable areas
            {
                randPos = hit.position;
                Debug.DrawRay(hit.position, Vector3.up, Color.blue, 1.0f);
            }
            bIsPosValid = false;
        }
        return randPos;
    }


}
