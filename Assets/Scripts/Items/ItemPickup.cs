using System;
using Combat;
using UnityEngine;

public class ItemPickup : Interactable {
    public Item item;

    public override void Interact() {
        base.Interact();

        //GameManager.Instance.player.GetComponent<Fighter>().RNGWeapon = ScriptableObject.CreateInstance<Weapon>();

        PickUp();
    }

    private void PickUp() {
        //Debug.Log("Picking up " + item.name + " !");

        if (item.GetType() == typeof(Equipment)) {
        }

        var wasPickedUp = Inventory.instance.Add(item);
        if (wasPickedUp)
            Destroy(transform.gameObject);
    }
}