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

    public bool AddItem(Item2 _item, int _amount) {
        if (EmptySlotCount <= 0)
            return false;
        InventorySlot2 slot = FindItemOnInventory(_item);
        if(!database.Items[_item.Id].stackable || slot == null)
        {
            SetEmptySlot(_item, _amount);
            return true;
        }
        slot.AddAmount(_amount);
        return true;
    }
    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < Container.Items.Length; i++)
            {
                if (Container.Items[i].item.Id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
    public InventorySlot2 FindItemOnInventory(Item2 _item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if(Container.Items[i].item.Id == _item.Id)
            {
                return Container.Items[i];
            }
        }
        return null;
    }

    public InventorySlot2 SetEmptySlot(Item2 _item, int _amount) {
        for (int i = 0; i < Container.Items.Length; i++) {
            if (Container.Items[i].item.Id <= -1) {
                Container.Items[i].UpdateSlot(_item, _amount);
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
                Container.Items[i].UpdateSlot( newContainer.Items[i].item, newContainer.Items[i].amount);
            }
            stream.Close();
        }
        Debug.Log("Inventory successfully loaded");
    }

    [ContextMenu("Clear")]
    public void Clear() {
        Container.Clear();
    }
    
    public void SwapItems(InventorySlot2 item1, InventorySlot2 item2) {

        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot2 temp = new InventorySlot2(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }
    }

    public void RemoveItem(Item2 _item) {
        for (int i = 0; i < Container.Items.Length; i++) {
            if (Container.Items[i].item == _item) {
                Container.Items[i].UpdateSlot( null, 0);
            }
        }
    }
}

[System.Serializable]
public class Inventory2 {
    public InventorySlot2[] Items = new InventorySlot2[24];
    
    public void Clear() {
        for (int i = 0; i < Items.Length; i++) {
            Items[i].RemoveItem();
        }
    }
}

[System.Serializable]
public class InventorySlot2 {
    
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized]
    public UserInterface parent;
    public Item2 item = new Item2();
    public int amount;

    public ItemObject ItemObject
    {
        get
        {
            if(item.Id >= 0)
            {
                return parent.inventory.database.Items[item.Id];
            }
            return null;
        }
    }
    // Ctor
    public InventorySlot2() {
        item = new Item2();
        amount = 0;
    }
    
    // Ctor
    public InventorySlot2(Item2 _item, int _amount) {
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value) {
        amount += value;
    }
    
    public void UpdateSlot(Item2 _item, int _amount) {
        item = _item;
        amount = _amount;
    }

    public void RemoveItem()
    {
        item = new Item2();
        amount = 0;
    }
    public bool CanPlaceInSlot(ItemObject _itemObject) {
        if (AllowedItems.Length <= 0 || _itemObject == null || _itemObject.data.Id < 0)
            return true;

        for (int i = 0; i < AllowedItems.Length; i++) {
            if (_itemObject.type == AllowedItems[i])
                return true;
        }

        return false;
    }
}