using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUnfaderEnding : StateMachineBehaviour
{
    public void UnfadeEnding()
    {
        GameManager.Instance.uiManager.PurgeMenuGO.GetComponent<PurgeMenu>().PurgeEnding.GetComponent<CanvasGroup>().alpha = 1;
    }
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UnfadeEnding();
    }
}
