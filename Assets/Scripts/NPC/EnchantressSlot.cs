using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;
using UnityEngine.UI;

public class EnchantressSlot : InventorySlot
{
    public override void AddItem(Item newItem) {
        base.AddItem(newItem);
    }

    public override void ClearSlot() {
        base.ClearSlot();
    }

    public override void OnRemoveButton() {
        base.OnRemoveButton();
    }

    // public override void UseItem()
    // {
    //     if (item != null) {
    //         GameManager.Instance.uiManager.EnchantressGO.GetComponent<EnchantressUI>()
    //             .EnchantressMainSlotButton.GetComponent<EnchantressMainSlot>().AddItem(item);
    //         item.RemoveFromInventory();
    //     }
    // }
}
