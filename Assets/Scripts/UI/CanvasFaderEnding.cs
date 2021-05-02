using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFaderEnding : StateMachineBehaviour
{
    public void FadeEnding()
    {
        GameManager.Instance.uiManager.PurgeMenuGO.GetComponent<PurgeMenu>().PurgeEnding.GetComponent<CanvasGroup>()
            .alpha = 0;
    }
    
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FadeEnding();
    }
}
