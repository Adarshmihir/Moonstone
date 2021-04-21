using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventoryEnchantressUI : MonoBehaviour
{
    public InventoryObject inventory;

    public Transform itemsParent;

    private EnchantressSlot[] slots;
    
    

    // Start is called before the first frame update
    public void Initialize_InventoryEnchantressUI() {
        // inventory.onItemChangedCallback += updateUI;

        // slots = itemsParent.GetComponentsInChildren<EnchantressSlot>();
        
        
    }

    // Update is called once per frame
    void Update() {
 
    }

    // void updateUI() {
    //     for (int i = 0; i < slots.Length; i++) {
    //         if (i < inventory.items.Count) {
    //             slots[i].AddItem(inventory.items[i]);
    //         }
    //         else {
    //             slots[i].ClearSlot();
    //         }
    //     }
    // }
    
    
    
    // public void displayStats(InventorySlot2 _slot) {
    //     foreach (var buff in _slot.item.buffs) {
    //         Instantiate(createStatText(buff), Vector3.zero, Quaternion.identity, statContainer);
    //         // statRecap.text += "\n" + buff.attribute + " : " + buff.value;
    //     }
    // }
    //
    // public GameObject createStatText(ItemBuff _buff) {
    //     var g = new GameObject();
    //     g.name = "Text";
    //     var rt = g.AddComponent<RectTransform>();
    //     Text txt = g.AddComponent<Text>();
    //     txt.text = _buff.attribute + " : " + _buff.value;
    //     txt.font = font;
    //
    //     return g;
    // }
    
    
}
