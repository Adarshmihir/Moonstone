using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFaderOpening : StateMachineBehaviour
{
    public void FadeOpening()
    {
        GameManager.Instance.uiManager.PurgeMenuGO.GetComponent<PurgeMenu>().PurgeOpening.GetComponent<CanvasGroup>().alpha = 0;
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FadeOpening();
    }
}
