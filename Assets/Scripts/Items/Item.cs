using System.Collections.Generic;
using Combat;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [SerializeField] private Spell spell;
    public new string name = "New Item";
    public Sprite icon;
    public bool isDefaultItem;
    
    public equipementModifier[] equipementMods;

    public EquipmentSlot equipSlot;

    public Spell Spell => spell;

    public virtual void Use() {
        Debug.Log("Using " + name);
    }

    public void RemoveFromInventory() {
        Inventory.instance.Remove(this);
    }
}

public enum EquipmentSlot { Head, Body, Legs, Foot, Weapon}