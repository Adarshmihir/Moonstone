using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forgeron : Interactable
{

    protected override void checkFocus()
    {
        base.checkFocus();
        if (hasInteracted)
        {
            var distance = Vector3.Distance(base.player.position, interactionTransform.position);
            if (distance > radius)
            {
                if (GameManager.Instance.uiManager.ForgeronGO.activeSelf == true)
                {
                    GameManager.Instance.uiManager.ForgeronGO.SetActive(false);
                }
                this.OnDefocused();
            }
        }
    }

    public override void Interact()
    {
        base.Interact();
        if (GameManager.Instance.uiManager.ForgeronGO.activeSelf == false)
        {
            GameManager.Instance.uiManager.ForgeronGO.SetActive(true);
        }
    }
}

