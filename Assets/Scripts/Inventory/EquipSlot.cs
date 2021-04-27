using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

public class EquipSlot : InventorySlot
{/*
    public override void AddItem(Item newItem)
    {
    if (newItem is Equipment || newItem is Weapon)
        {
            if (!item)
            {
                base.AddItem(newItem);
            }
            else
            {
                base.AddItem(newItem);
            }
        }
    }

    public override void ClearSlot()
    {
        base.ClearSlot();
    }

    public override void OnRemoveButton()
    {
        if (item != null)
        {
            if (item is Equipment)
            {
                foreach (var mod in ((Equipment) item).StatModifiers)
                {
                GameManager.Instance.player.RemoveModifier(mod);
                }
            }
        else
        {
            foreach (var mod in ((Weapon)item).StatModifiers)
            {
                GameManager.Instance.player.RemoveModifier(mod);
            }
        } 
        }
        GameManager.Instance.equipementManager.Unequip((int)item.equipSlot);
        base.ClearSlot();
    }

    public override void UseItem()
    {
    }*/

}
