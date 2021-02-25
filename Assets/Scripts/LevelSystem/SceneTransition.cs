using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Movement;
using UnityEngine.AI;

public class SceneTransition : MonoBehaviour
{
    public Transform teleportationPoint;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Mover>().StartMoveAction(teleportationPoint.position);
            other.GetComponent<NavMeshAgent>().Warp(teleportationPoint.position);
            //other.transform.position = playerPosition;
        }
    }
}
