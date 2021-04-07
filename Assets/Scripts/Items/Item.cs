using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    public new string name = "New Item";
    public Sprite icon;
    public string desc = "";
    
    //public equipementModifier[] equipementMods;

    // Inutile tant que l'on ne place pas les items sur le joueur
    public EquipmentSlot equipSlot;


    public virtual void Use() {
        Debug.Log("Using " + name);
    }

    public void RemoveFromInventory() {
        Inventory.instance.Remove(this);
    }
    
    // Inutile tant que l'on ne place pas les items sur le joueur
    public enum EquipmentSlot { Head, Body, Legs, Foot, Weapon, None}
}

