using System.Collections;
using System.Collections.Generic;
using System.IO;
using Combat;
using JetBrains.Annotations;
using UnityEngine;

public enum ItemType {
    Helmet,
    Chest,
    Legs,
    Weapon,
    UniqueWeapon,
    DualWeapon,
    DoubleHandWeapon,
    Shield,
    Resource,
    Disabled,
    Default
}


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/item")]

public class ItemObject : ScriptableObject {

    public Sprite uiDisplay;
    //use by armor or double hand weapon
    public GameObject characterDisplay;
    //use by one hand weapon 
    public GameObject characterDisplayRight;
    public GameObject characterDisplayLeft;
    [SerializeField] private AnimatorOverrideController animatorOverride;
    [SerializeField] private float weaponRange = 2f;
    public bool stackable;
    public ItemType[] type;
    public int minFlat;
    public int maxFlat;
    [SerializeField] [Range(0f, 180f)] private float weaponRadius = 45f;
    [TextArea(15, 20)] public string description;
    [TextArea(4, 20)] public string recapStats; 
    public Item2 data = new Item2();
    [SerializeField] private Spell spell;
    public Spell Spell => spell;
    
    public AnimatorOverrideController AnimatorOverride=> animatorOverride;
    public float WeaponRange => weaponRange;
    public float WeaponRadius => weaponRadius;
    public Item2 CreateItem() {
        Item2 newItem = new Item2(this);
        return newItem;
    }
    
}

[System.Serializable]
public class Item2 {
    public string Name;
    public int Id = -1;
    public int valueFlat;
    public StatTypes damageBuffStat;
    public float damageBuffValue;
    public ItemBuff[] buffs;


    public Item2() {
        Name = "";
        Id = -1;
    }
    
    // Ctor
    public Item2(ItemObject item) {
        Name = item.name;
        Id = item.data.Id;
        buffs = new ItemBuff[item.data.buffs.Length];
        valueFlat = Random.Range(item.minFlat, item.maxFlat);
        damageBuffValue = item.data.damageBuffValue;
        for (int i = 0; i < buffs.Length; i++) {
            buffs[i] = new ItemBuff(item.data.buffs[i].min, item.data.buffs[i].max);
            buffs[i].attribute = item.data.buffs[i].attribute;
        }
    }
}
[System.Serializable]
public class ItemBuff : IModifier{
    public StatTypes attribute;
    public int value;
    public int min;
    public int max;
    private IModifier modifierImplementation;

    public ItemBuff(int _min, int _max) {
        min = _min;
        max = _max;
        GenerateValue();
    }

    public void GenerateValue() {
        value = Random.Range(min, max);
    }

    public void AddValue(ref int baseValue)
    {
        baseValue += value;
    }
    
}