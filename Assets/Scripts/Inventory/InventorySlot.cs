using System;
using Combat;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
    private Item item;
    private Equipment equipment;
    private Weapon weapon;
    private string itemTypeString;

    // UI
    public Image icon;
    public Button removeButton;

    public void AddItem(Item newItem) {
        SetItem(newItem);

        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public void ClearSlot() {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton() {
        Inventory.instance.Remove(item);
    }

    public void Use() {       
        if (item != null)
            item.Use();
    }
    
    
    // Inutile pour le moment puisque les comportements sont les memes entre Weapon, Equipment etc .. vis a vis de l'inventaire
    //TODO: IDEE POLISH --> Mettre un background different pour chaque type d'objet. (Ex. Vert pour les plantes, Rouge pour les armes, ...) -> la ca serait utile
    private Item SetItem(Item newItem) {
        itemTypeString = newItem.GetType().ToString();
        
        switch (itemTypeString) {
            case "Combat.Weapon" :
                weapon = (Weapon)newItem;
                return item = weapon;
            
            case "Equipment":
                equipment = (Equipment) newItem;
                return item = equipment;
            
            default:
                return item = newItem;
        }
    }

    
}
