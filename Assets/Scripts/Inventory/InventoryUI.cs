using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {
    private Inventory inventory;

    public Transform itemsParent;

    private InventorySlot[] slots;

    // Start is called before the first frame update
    public void Initialize_InventoryUI() {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += updateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
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
