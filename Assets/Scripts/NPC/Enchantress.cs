using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enchantress : Interactable {
    public InventoryObject inventory;

    public Button resetButton;
    public Button increaseButton;
    
    protected override void checkFocus() {
        base.checkFocus();
        if (hasInteracted) {
            var distance = Vector3.Distance(base.player.position, interactionTransform.position);
            if (distance > radius) {
                if (GameManager.Instance.uiManager.EnchantressGO.activeSelf == true) {
                    GameManager.Instance.uiManager.EnchantressGO.SetActive(false);
                    GameManager.Instance.uiManager.EnchantressGO.GetComponent<EnchantressUI>().EnchantressMainSlotButton
                        .GetComponent<EnchantressMainSlot>().OnRemoveButton();
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

    public void Start() {
        inventory.GetSlots[0].OnBeforeUpdate += OnBeforeSlotUpdate;
        inventory.GetSlots[0].OnAfterUpdate += OnAfterSlotUpdate;
    }

    public void OnBeforeSlotUpdate(InventorySlot2 _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                Debug.Log("interacting with inventory : before");
                break;
            case InterfaceType.Equipment:

                break;
            case InterfaceType.Chest:
                break;
            
            case InterfaceType.Enchantress:
                Debug.Log("interacting with enchantress : before");
                break;
            default:
                break;
        }
    }
    public void OnAfterSlotUpdate(InventorySlot2 _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                Debug.Log("interacting with inventory : after");
                break;
            case InterfaceType.Equipment:

                break;
            case InterfaceType.Chest:
                break;
            
            case InterfaceType.Enchantress:
                Debug.Log("interacting with enchantress : after");
                foreach (var buff in _slot.item.buffs) {
                    Debug.Log(buff.value);
                }
                
                break;
            default:
                break;
        }
    }
    
    private void OnApplicationQuit() {
        inventory.Container.Clear();
    }
    
    public void IncreaseStat() {
    }

    public void ResetStat() {
        
    }
}