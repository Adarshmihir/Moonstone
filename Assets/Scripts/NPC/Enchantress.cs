using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enchantress : Interactable {
    public InventoryObject inventory;
    public void Start() {
        // inventory.GetSlots[0].OnBeforeUpdate += OnBeforeSlotUpdate;
        // inventory.GetSlots[0].OnAfterUpdate += OnAfterSlotUpdate;
    }

    protected override void checkFocus() {
        base.checkFocus();
        if (hasInteracted) {
            var distance = Vector3.Distance(base.player.position, interactionTransform.position);
            if (distance > radius) {
                if (GameManager.Instance.uiManager.EnchantressGO.activeSelf == true) {
                    GameManager.Instance.uiManager.EnchantressGO.SetActive(false);
                    // GameManager.Instance.uiManager.EnchantressGO.GetComponent<EnchantressUI>().EnchantressMainSlotButton
                    //      .GetComponent<EnchantressMainSlot>().OnRemoveButton();
                }

                this.OnDefocused();
            }
        }
    }

    public override void Interact() {
        base.Interact();
        if (GameManager.Instance.uiManager.EnchantressGO.activeSelf == false) {
            GameManager.Instance.uiManager.EnchantressGO.SetActive(true);
            GameManager.Instance.uiManager.EnchantressGO.GetComponent<EnchantressUI>().inventoryenchantress
                .Initialize_InventoryEnchantressUI();
        }
    }


    // public void OnBeforeSlotUpdate(InventorySlot2 _slot) {
    //     if (_slot.ItemObject == null)
    //         return;
    //     switch (_slot.parent.inventory.type) {
    //         case InterfaceType.Inventory:
    //             Debug.Log("interacting with inventory : before");
    //             break;
    //         case InterfaceType.Equipment:
    //
    //             break;
    //         case InterfaceType.Chest:
    //             break;
    //
    //         case InterfaceType.Enchantress:
    //             Debug.Log("interacting with enchantress : before");
    //             // statRecap.enabled = false;
    //             // statRecap.text = "";
    //             foreach (var stat in statContainer.GetComponentsInChildren<Text>()) {
    //                 Destroy(stat.gameObject);
    //             }
    //
    //             break;
    //         default:
    //             break;
    //     }
    // }
    //
    // public void OnAfterSlotUpdate(InventorySlot2 _slot) {
    //     if (_slot.ItemObject == null)
    //         return;
    //     switch (_slot.parent.inventory.type) {
    //         case InterfaceType.Inventory:
    //             Debug.Log("interacting with inventory : after");
    //             break;
    //         case InterfaceType.Equipment:
    //
    //             break;
    //         case InterfaceType.Chest:
    //             break;
    //
    //         case InterfaceType.Enchantress:
    //             Debug.Log("interacting with enchantress : after");
    //
    //             // statRecap.enabled = true;
    //
    //
    //             displayStats(_slot);
    //
    //             break;
    //         default:
    //             break;
    //     }
    // }

    // public void displayStats(InventorySlot2 _slot) {
    //     foreach (var buff in _slot.item.buffs) {
    //         Instantiate(ModButton, Vector3.zero, Quaternion.identity, statContainer);
    //         // statRecap.text += "\n" + buff.attribute + " : " + buff.value;
    //     }
    // }

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

    private void OnApplicationQuit() {
        inventory.Container.Clear();
    }

    public void IncreaseStat() { }

    // public void ResetStat() {
    //     var enchantressSlot = FindObjectOfType<Enchantress>().inventory.GetSlots[0];
    //     var buffs = enchantressSlot.item.buffs;
    //
    //     // Randomize value and attribute of the new stat
    //     foreach (var buff in buffs) {
    //         buff.attribute = (StatTypes) Enum.ToObject(typeof(StatTypes), Random.Range(0, 4));
    //         buff.value = Random.Range(buff.min, buff.max);
    //     }
    //
    //     //currently printing in console, TODO: display changes in enchantress mod list 
    //     // statRecap.text = "\n" + buff.attribute + " : " + buff.value;
    //
    //     // Clear all stats
    //     foreach (var txt in statContainer.GetComponentsInChildren<Text>()) {
    //         Destroy(txt.gameObject);
    //     }
    //
    //     // Display all updated stats
    //     displayStats(enchantressSlot);
    // }
}