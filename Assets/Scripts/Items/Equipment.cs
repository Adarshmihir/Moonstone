using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item {

    public EquipmentSlot equipSlot;
    public SkinnedMeshRenderer mesh;
    public EquipmentMeshRegion[] coveredMeshRegions;
    
    public int armorModifier;
    public int damageModifier;

    public override void Use() {
        base.Use();
        // Equip the item
        EquipmentManager.instance.Equip(this);
        
        // Remove it from the inventory
        RemoveFromInventory();
    }
}

public enum EquipmentSlot { Head, Body, Legs, Foot, Weapon}
public enum EquipmentMeshRegion {Legs, Arms, Torso} // Corresponds to body blendshapes


