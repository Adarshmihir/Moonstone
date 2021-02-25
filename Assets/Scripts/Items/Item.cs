using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    public new string name = "New Item";
    public Sprite icon;
    public bool isDefaultItem;
    
    public equipementModifier[] equipementMods;

    
    public EquipmentSlot equipSlot;

    public virtual void Use() {
        Debug.Log("Using " + name);
    }

    public void RemoveFromInventory() {
        Inventory.instance.Remove(this);
    }
}

public enum EquipmentSlot { Head, Body, Legs, Foot, Weapon}