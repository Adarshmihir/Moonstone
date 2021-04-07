using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player2 : MonoBehaviour {
    public InventoryObject inventory;
    public MouseItem mouseItem = new MouseItem();

    public void OnTriggerEnter(Collider other) {
        var item = other.GetComponent<GroundItem>();
        if (item) {
            inventory.AddItem(new Item2(item.item), 1);
            Destroy(other.gameObject);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            inventory.Save();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
            inventory.Load();
        }
    }

    private void OnApplicationQuit() {
        inventory.Container.Items = new InventorySlot2[24];
    }
}