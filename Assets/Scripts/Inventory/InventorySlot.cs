using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour {
    protected Item item;
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
        if (item != null) {
            item.Use();
        }
    }
}
