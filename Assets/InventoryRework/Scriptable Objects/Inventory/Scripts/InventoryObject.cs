using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject {
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory2 Container;

    public void AddItem(Item2 _item, int _amount) {
        // If different buffs, a new item is added to the inventory
        if (_item.buffs.Length > 0) {
            SetEmptySlot(_item, _amount);
            return;
        }
        
        // else, items are stacked
        for (int i = 0; i < Container.Items.Length; i++) {
            if (Container.Items[i].ID == _item.Id) {
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }
        SetEmptySlot(_item, _amount);
    }

    public InventorySlot2 SetEmptySlot(Item2 _item, int _amount) {
        for (int i = 0; i < Container.Items.Length; i++) {
            if (Container.Items[i].ID <= -1) {
                Container.Items[i].UpdateSlot(_item.Id, _item, _amount);
                return Container.Items[i];
            }
        }
        // Manage full inventory here
        return null;
    }

    [ContextMenu("Save")]
    public void Save() {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create,
            FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
        Debug.Log("Inventory successfully saved");
    }

    [ContextMenu("Load")]
    public void Load() {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath))) {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory2 newContainer = (Inventory2) formatter.Deserialize(stream);
            for (int i = 0; i < Container.Items.Length; i++) {
                Container.Items[i].UpdateSlot(newContainer.Items[i].ID, newContainer.Items[i].item, newContainer.Items[i].amount);
            }
            stream.Close();
        }
        Debug.Log("Inventory successfully loaded");
    }

    [ContextMenu("Clear")]
    public void Clear() {
        Container.Clear();
    }
    
    public void MoveItem(InventorySlot2 item1, InventorySlot2 item2) {
        InventorySlot2 temp = new InventorySlot2(item2.ID, item2.item, item2.amount);
        item2.UpdateSlot(item1.ID, item1.item, item1.amount);
        item1.UpdateSlot(temp.ID, temp.item, temp.amount);
    }

    public void RemoveItem(Item2 _item) {
        for (int i = 0; i < Container.Items.Length; i++) {
            if (Container.Items[i].item == _item) {
                Container.Items[i].UpdateSlot(-1, null, 0);
            }
        }
    }
}

[System.Serializable]
public class Inventory2 {
    public InventorySlot2[] Items = new InventorySlot2[24];
    
    public void Clear() {
        for (int i = 0; i < Items.Length; i++) {
            Items[i].UpdateSlot(-1, new Item2(), 0);
        }
    }
}

[System.Serializable]
public class InventorySlot2 {
    public ItemType[] AllowedItems = new ItemType[0];
    public UserInterface parent;
    public int ID = -1;
    public Item2 item;
    public int amount;

    // Ctor
    public InventorySlot2() {
        ID = -1;
        item = null;
        amount = 0;
    }
    
    // Ctor
    public InventorySlot2(int _id, Item2 _item, int _amount) {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value) {
        amount += value;
    }
    
    public void UpdateSlot(int _id, Item2 _item, int _amount) {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public bool CanPlaceInSlot(ItemObject _item) {
        if (AllowedItems.Length <= 0)
            return true;

        for (int i = 0; i < AllowedItems.Length; i++) {
            if (_item.type == AllowedItems[i])
                return true;
        }

        return false;
    }
}