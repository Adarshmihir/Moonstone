using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Quests;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
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
            AddEvent(obj, EventTriggerType.PointerClick, delegate { OnClick(obj); });
            inventory.GetSlots[i].slotDisplay = obj;
            if (GetComponent<UI_Forgeron>())
            {
                obj.GetComponentInChildren<Text>().text = inventory.GetSlots[i].ItemObject.CostGold.ToString();
            }
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

        buff.attribute = (StatTypes) Enum.ToObject(typeof(StatTypes), Random.Range(0, 4));
        buff.value = Random.Range(buff.min, buff.max);

        Debug.Log(buff.attribute);
    }

    public override void UseItem() {
        switch (inventory.type) {
            case InterfaceType.Enchantress:
                Debug.Log("Enchantress item clicked !");
                break;

            case InterfaceType.Forgeron:
                Debug.Log("Forgeron item clicked !");
                break;
        }
    }
}