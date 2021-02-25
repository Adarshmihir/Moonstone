using System;
using Combat;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
	protected Item item;
    private Equipment equipment;
    private Weapon weapon;
    private string itemTypeString;

    // UI
    public Image icon;
    public Button removeButton;

    public virtual void AddItem(Item newItem) {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    public virtual void ClearSlot() {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public virtual void OnRemoveButton() {
        Inventory.instance.Remove(item);
    }

    public virtual void UseItem() {
        if (item != null)
			item.Use();
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
