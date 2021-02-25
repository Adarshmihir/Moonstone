using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantressMainSlot : InventorySlot
{
    public override void AddItem(Item newItem) {
        base.AddItem(newItem);
        GameManager.Instance.uiManager.EnchantressGO.GetComponent<EnchantressUI>().OnItemInMainSlot(newItem);
    }

    public override void ClearSlot() {
        base.ClearSlot();
    }

    public override void OnRemoveButton() {
        if (item != null)
        {
            Inventory.instance.Add(item);
        }
        base.ClearSlot();
        GameManager.Instance.uiManager.EnchantressGO.GetComponent<EnchantressUI>().ClearModifierList();
    }

    public override void UseItem()
    {
    }
}
