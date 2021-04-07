using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

public enum ItemType {
    Chest,
    Weapon,
    Helmet,
    Shield,
    Legs,
    Resource,
    Default
}

public enum Attributes {
    Strength,
    Stamina,
    Intelligence,
    Perception,
    Agility
}

[System.Serializable]
public abstract class ItemObject : ScriptableObject {
    public int Id;
    public Sprite uiDisplay;
    public ItemType type;

    [TextArea(15, 20)] public string description;
    public ItemBuff[] buffs;

    public Item2 CreateItem() {
        Item2 newItem = new Item2(this);
        return newItem;
    }
}

[System.Serializable]
public class Item2 {
    public string Name;
    public int Id;
    public ItemBuff[] buffs;


    public Item2() {
        Name = "";
        Id = -1;
    }
    
    // Ctor
    public Item2(ItemObject item) {
        Name = item.name;
        Id = item.Id;
        buffs = new ItemBuff[item.buffs.Length];
        for (int i = 0; i < buffs.Length; i++) {
            buffs[i] = new ItemBuff(item.buffs[i].min, item.buffs[i].max);
            buffs[i].attribute = item.buffs[i].attribute;
        }
    }
}

[System.Serializable]
public class ItemBuff {
    public Attributes attribute;
    public int value;
    public int min;
    public int max;

    public ItemBuff(int _min, int _max) {
        min = _min;
        max = _max;
        GenerateValue();
    }

    public void GenerateValue() {
        value = Random.Range(min, max);
    }
}