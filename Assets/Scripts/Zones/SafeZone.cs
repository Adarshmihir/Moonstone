using Control;
using UnityEngine;

public class SafeZone : Zone
{
    protected override void UseTriggerZone(Collider col)
    {
        base.UseTriggerZone(col);
        
        if (!col.CompareTag("Enemy")) return;
        
        col.gameObject.GetComponent<AIController>().ReturnToInitialPosition();
    }
}
