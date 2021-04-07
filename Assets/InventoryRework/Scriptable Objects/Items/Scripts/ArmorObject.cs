using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Armor Object", menuName = "Inventory System/Items/Armor")]
public class ArmorObject : ItemObject {
    
    // Place StatsModifiers here
    // ...
    
    public enum Attributes {
        Strength,
        Stamina,
        Intelligence,
        Perception,
        Agility
    }
    
    private void Awake() {
        type = ItemType.Chest;
    }
}
