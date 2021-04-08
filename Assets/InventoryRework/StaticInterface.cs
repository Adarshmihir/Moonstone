using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class StaticInterface : UserInterface {
    public GameObject[] slots;
    public override void CreateSlots() {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot2>();
        for (int i = 0; i < inventory.GetSlots.Length; i++) {
            var obj = slots[i];
            
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            inventory.GetSlots[i].slotDisplay = obj;
            slotsOnInterface.Add(obj, inventory.GetSlots[i]);
        }
        
        
    }
    
    public void IncreaseStat() {
        var buff = FindObjectOfType<Enchantress>().inventory.GetSlots[0].item.buffs[0];
        buff.value += Random.Range(buff.min, buff.max);
        
        //currently printing in console, TODO: display changes in enchantress mod list 
    }

    public void ResetStat() {
        var buff = FindObjectOfType<Enchantress>().inventory.GetSlots[0].item.buffs[0];

        buff.attribute = (Attributes) Enum.ToObject(typeof(Attributes), Random.Range(0, 4));
        buff.value = Random.Range(buff.min, buff.max);

        Debug.Log(buff.attribute);

        //currently printing in console, TODO: display changes in enchantress mod list 
    }

    public override void UseItem() {

        Debug.Log("VAR");
    }
}