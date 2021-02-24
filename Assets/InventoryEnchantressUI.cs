using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEnchantressUI : MonoBehaviour
{
    private Inventory inventory;

    public Transform itemsParent;

    private EnchantressSlot[] slots;

    // Start is called before the first frame update
    public void Initialize_InventoryEnchantressUI() {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += updateUI;

        slots = itemsParent.GetComponentsInChildren<EnchantressSlot>();
    }

    // Update is called once per frame
    void Update() {
 
    }

    void updateUI() {
        for (int i = 0; i < slots.Length; i++) {
            if (i < inventory.items.Count) {
                slots[i].AddItem(inventory.items[i]);
            }
            else {
                slots[i].ClearSlot();
            }
        }
    }
}
