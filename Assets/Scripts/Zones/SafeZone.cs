using System.Collections;
using System.Collections.Generic;
using Control;
using UnityEngine;

public class SafeZone : Zone
{
    
    protected override void UseTriggerZone(Collider col)
    {
        base.UseTriggerZone(col);
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<AIController>().ReturnToInitialPosition();
        }
    }

}
