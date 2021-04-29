using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUnfaderOpening : StateMachineBehaviour
{
    public void UnfadeOpening()
    {
        GameManager.Instance.uiManager.PurgeMenuGO.GetComponent<PurgeMenu>().PurgeOpening.GetComponent<CanvasGroup>().alpha = 1;
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UnfadeOpening();
    }
}
