using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Control;
using Stats;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public float life;
    public float mana;
    public List<Stat> stats;
    public int level;


    public bool isInDungeon = false;
    public bool hasKilledABoss = false;

    public InventoryObject inventory;
    public InventoryObject equipment;
    public Attribute[] attributes;

    private void Start() {
        for (int i = 0; i < attributes.Length; i++) {
            attributes[i].SetParent(this);
        }

        for (int i = 0; i < equipment.GetSlots.Length; i++) {
            equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            inventory.Save();
            equipment.Save();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
            inventory.Load();
            equipment.Load();
        }
    }

    public void InitializeStats() {
        stats = new List<Stat>();
        foreach (StatTypes stat in Enum.GetValues(typeof(StatTypes))) {
            Stat statToAdd = new Stat(new CharacterStat(5),
                GameManager.Instance.uiManager.StatsCanvasGO.GetComponent<StatList>().getNumberGameObject(stat), stat);
            stats.Add(statToAdd);
        }

        StatTextUpdate();
    }

    public void InitializePlayer() {
        level = 1;
        InitializeStats();
    }

    public void AddModifier(StatModifier statMod) {
        foreach (var stat in stats) {
            if (stat.StatName == statMod.statType) {
                stat.charStat.AddModifier(statMod);
            }
        }

        StatTextUpdate();
    }

    public void StatTextUpdate() {
        foreach (var stat in stats) {
            if (stat.statGameObject) {
                if (stat.charStat.BaseValue == stat.charStat.Value) {
                    stat.statGameObject.GetComponent<Text>().text = stat.charStat.BaseValue.ToString();
                }
                else {
                    stat.statGameObject.GetComponent<Text>().text = stat.charStat.BaseValue.ToString() + " (+" +
                                                                    (stat.charStat.Value - stat.charStat.BaseValue)
                                                                    .ToString() + ")";
                }
            }
        }
    }

    public void AddPointToStat() {
        string name = EventSystem.current.currentSelectedGameObject.name;
        name = name.Replace("_Button", "");
        foreach (var stat in stats) {
            if (stat.StatName.ToString() == name) {
                StatList statList = GameManager.Instance.uiManager.StatsCanvasGO.GetComponent<StatList>();
                stat.charStat.IncrementBaseValue(2);
                statList.lvlup_Points -= 1;
                statList.PointsToSpendTextUpdate(statList.lvlup_Points);
                if (statList.lvlup_Points == 0) {
                    statList.ToggleLevelUp(false);
                }

                StatTextUpdate();
            }
        }
    }

    public void ResetStat() {
        foreach (Stat stat in stats) {
            stat.charStat.ResetBaseValue(5);
        }

        GameManager.Instance.uiManager.StatsCanvasGO.GetComponent<StatList>().ToggleReset(level);
        this.StatTextUpdate();
    }

    public void OnTriggerEnter(Collider other) {
        var item = other.GetComponent<GroundItem>();
        if (item) {
            Item2 _item = new Item2(item.item);
            if (inventory.AddItem(_item, 1)) {
                Destroy(other.gameObject);
            }
        }
    }

    private void OnApplicationQuit() {
        inventory.Container.Clear();
        equipment.Container.Clear();
    }
    
    public void OnBeforeSlotUpdate(InventorySlot2 _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            
            case InterfaceType.Enchantress:
                Debug.Log("VAR");
                break;
            default:
                break;
        }
    }
    public void OnAfterSlotUpdate(InventorySlot2 _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            
            case InterfaceType.Enchantress:
                break;
            default:
                break;
        }
    }
    
    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }
}

[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public Player parent;
    public Attributes type;
    public ModifiableInt value;
    
    public void SetParent(Player _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }
    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}