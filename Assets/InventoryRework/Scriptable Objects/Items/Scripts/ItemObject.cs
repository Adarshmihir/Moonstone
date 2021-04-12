using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

public enum ItemType {
    Chest,
    Weapon,
    Right,
    Left,
    WeaponDouble,
    Helmet,
    Shield,
    Legs,
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
    [SerializeField] private AnimatorOverrideController animatorOverrideBothHand;
    [SerializeField] private AnimatorOverrideController animatorOverrideLeftHand;
    [SerializeField] private AnimatorOverrideController animatorOverrideRightHand;
    
    public bool stackable;
    public ItemType type;
    public int minFlat;
    public int maxFlat;
    [TextArea(15, 20)] public string description;
    public Item2 data = new Item2();

    
    public AnimatorOverrideController AnimatorOverrideBothHand => animatorOverrideBothHand;
    public AnimatorOverrideController AnimatorOverrideRightHand => animatorOverrideRightHand;
    public AnimatorOverrideController AnimatorOverrideLeftHand => animatorOverrideLeftHand;
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
        Debug.Log(valueFlat);
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