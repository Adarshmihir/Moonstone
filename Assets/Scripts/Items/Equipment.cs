using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct equipementModifier
{
    public float value;
    public StatModType modType;
    public StatTypes statType;
}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item {

    public EquipmentSlot equipSlot;
    public SkinnedMeshRenderer mesh;
    public EquipmentMeshRegion[] coveredMeshRegions;

    public equipementModifier[] equipementMods;

    //A PASSER EN PROTECTED QUAND ON AURA FAIT UN NAMESPACE INVENTAIRE-STATS
    public List<StatModifier> StatModifiers;

    private void OnEnable()
    {
        this.assignStatModifiers();
    }

    public override void Use() {
        base.Use();
        // Equip the item
        EquipmentManager.instance.Equip(this);
        foreach (var mod in StatModifiers)
        {
            GameManager.Instance.player.AddModifier(mod);
        }
        // Remove it from the inventory
        RemoveFromInventory();
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


