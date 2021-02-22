using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantressMainSlot : InventorySlot
{
    public override void AddItem(Item newItem) {
        base.AddItem(newItem);
        
    }

    public override void ClearSlot() {
        base.ClearSlot();
    }

    public override void OnRemoveButton() {
        Inventory.instance.Add(item);
        base.ClearSlot();
    }

    public override void UseItem()
    {
    }
}
