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
        if (!other.CompareTag("Player")) return;
        
        GameManager.Instance.player.isInDungeon = !GameManager.Instance.player.isInDungeon;
        StartCoroutine(TeleportPlayer(other));
    }

    private IEnumerator TeleportPlayer(Component other)
    {
        var loaderAnimator = GameManager.Instance.LoaderAnimator;
        loaderAnimator.SetTrigger("startFade");

        yield return new WaitForSeconds(1f);
            
        other.GetComponent<Mover>().StartMoveAction(teleportationPoint.position);
        other.GetComponent<NavMeshAgent>().Warp(teleportationPoint.position);
            
        yield return new WaitForSeconds(1f);
            
        loaderAnimator.SetTrigger("endFade");
    }
}
