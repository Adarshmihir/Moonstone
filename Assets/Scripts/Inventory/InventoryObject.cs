﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public enum InterfaceType
{
    Inventory,
    Equipment,
    Chest,
    Enchantress,
    Forgeron
}


[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject {
    public string savePath;
    public ItemDatabaseObject database;
    public InterfaceType type;
    public Inventory2 Container;
    public float initGold = 300f;
    public float gold = 300f;
    
    
    public InventorySlot2[] GetSlots { get { return Container.Slots; } }
    public bool AddItem(Item2 _item, int _amount) {
        if (EmptySlotCount <= 0)
            return false;
        
        InventorySlot2 slot = FindItemOnInventory(_item);
        
        if(!database.ItemObjects[_item.Id].stackable || slot == null)
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
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].item.Id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
    public InventorySlot2 FindItemOnInventory(Item2 _item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if(GetSlots[i].item.Id == _item.Id)
            {
                return GetSlots[i];
            }
        }
        return null;
    }

    public InventorySlot2 SetEmptySlot(Item2 _item, int _amount) {
        for (int i = 0; i < GetSlots.Length; i++) {
            if (GetSlots[i].item.Id <= -1) {
                GetSlots[i].UpdateSlot(_item, _amount);
                return GetSlots[i];
            }
        }
        // Manage full inventory here
        return null;
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
        for (int i = 0; i < GetSlots.Length; i++) {
            if (GetSlots[i].item == _item) {
                GetSlots[i].UpdateSlot( null, 0);
            }
        }
    }
    
    private void OnApplicationQuit() {
        gold = initGold;
    }
}

[System.Serializable]
public class Inventory2 {
    public InventorySlot2[] Slots = new InventorySlot2[24];
    
    public void Clear() {
        for (int i = 0; i < Slots.Length; i++) {
            Slots[i].RemoveItem();
        }
    }
}
public delegate void SlotUpdated(InventorySlot2 _slot);

[System.Serializable]
public class InventorySlot2 {
    
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized]
    public UserInterface parent;
    [System.NonSerialized]
    public GameObject slotDisplay;
    [System.NonSerialized]
    public SlotUpdated OnAfterUpdate;
    [System.NonSerialized]
    public SlotUpdated OnBeforeUpdate;
    
    public Item2 item = new Item2();
    public int amount;

    public ItemObject ItemObject
    {
        get
        {
            if(item.Id >= 0)
            {
                return parent.inventory.database.ItemObjects[item.Id];
            }
            return null;
        }
    }
    // Ctor
    public InventorySlot2() {
        UpdateSlot(new Item2(), 0);
    }
    
    // Ctor
    public InventorySlot2(Item2 _item, int _amount) {
        UpdateSlot(_item, _amount);
    }

    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
    }
    
    public void UpdateSlot(Item2 _item, int _amount) {
        if (OnBeforeUpdate != null)
            OnBeforeUpdate.Invoke(this);
        item = _item;
        amount = _amount;
        if (OnAfterUpdate != null)
            OnAfterUpdate.Invoke(this);
    }

    public void RemoveItem()
    {
        UpdateSlot(new Item2(), 0);
    }
    public bool CanPlaceInSlot(ItemObject _itemObject) {
        
        if (AllowedItems.Length <= 0 || _itemObject == null || _itemObject.data.Id < 0)
        {
            return true;

        }

        for (int i = 0; i < _itemObject.type.Length; i++)
        {
            if (_itemObject.type[i] == AllowedItems[0])
            {
                return true;
            }
        }

        return false;
    }
}