using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    public new string name = "New Item";
    public Sprite icon;
    public bool isDefaultItem;
    
    public equipementModifier[] equipementMods;

    private List<StatModifier> StatModifiers = new List<StatModifier>();

    private void OnEnable()
    {
        this.assignStatModifiers();
    }
    
    public EquipmentSlot equipSlot;

    public virtual void Use() {
        // Use the item
        foreach (var mod in StatModifiers)
        {
            GameManager.Instance.player.AddModifier(mod);
        }
        
        // Equip the selected item/equipment/weapon
        EquipmentManager.instance.Equip(this);
        
        // Remove it from the inventory
        RemoveFromInventory();
        
        Debug.Log("Using " + name);
    }

    public void RemoveFromInventory() {
        Inventory.instance.Remove(this);
    }
    
    public void assignStatModifiers()
    {
        StatModifiers.Clear();
        for (int i = 0; i < equipementMods.Length; i++)
        {
            StatModifier newMod = StatModifier.CreateInstance(equipementMods[i].value, equipementMods[i].modType, this ,equipementMods[i].statType);
            StatModifiers.Add(newMod);
        }
    }
}

public enum EquipmentSlot { Head, Body, Legs, Foot, Weapon}