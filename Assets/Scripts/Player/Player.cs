using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Control;
using Resources;
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
    [SerializeField] private AnimatorOverrideController animatorOverrideDefault;
    
    private Transform bootsTransform;
    private Transform chestTransform;
    private Transform helmetTransform;
    private Transform offhandTransform;
    private Transform weaponRightTransform;
    private Transform weaponLeftTransform;
    
    
    public Transform weaponTransformRight;
    public Transform weaponTransformLeft;


    private BoneCombiner boneCombiner;
    private Animator _animator;
    
    private void Start() {
        
        boneCombiner = new BoneCombiner(gameObject);
        
        for (int i = 0; i < attributes.Length; i++) {
            attributes[i].SetParent(this);
        }

        for (int i = 0; i < equipment.GetSlots.Length; i++) {
            equipment.GetSlots[i].OnBeforeUpdate += OnRemoveItem;
            equipment.GetSlots[i].OnAfterUpdate += OnAddItem;
        }
        _animator = GetComponent<Animator>();
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
    
    public void OnRemoveItem(InventorySlot2 _slot)
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
                if (_slot.ItemObject.characterDisplay != null || _slot.ItemObject.characterDisplayRight != null || _slot.ItemObject.characterDisplayLeft != null )
                {
                    switch (_slot.AllowedItems[0])
                    {
                        case ItemType.Helmet:
                            //Destroy(helmet.gameObject);
                            break;
                        case ItemType.Right:
                            Debug.Log("Right destroy");
                            switch (_slot.ItemObject.type)
                            {
                                case ItemType.Weapon:
                                    Destroy(weaponRightTransform.gameObject);
                                    weaponRightTransform = null;
                                    SetAnimatorPlayer(_slot);
                                    break;
                                case ItemType.WeaponDouble:
                                    Destroy(weaponRightTransform.gameObject);
                                    weaponRightTransform = null;
                                    weaponLeftTransform = null;
                                    _slot.parent.inventory.GetSlots[3].AllowedItems[0] = ItemType.Left;
                                    SetAnimatorPlayer(_slot);
                                    break;
                                default:
                                    Debug.Log("erreur");
                                    break;
                            }

                            break;
                        case ItemType.Left:
                            Debug.Log("Left destroy");
                            switch (_slot.ItemObject.type)
                            {
                                case ItemType.Weapon:
                                    Destroy(weaponLeftTransform.gameObject);
                                    weaponLeftTransform = null;
                                    SetAnimatorPlayer(_slot);
                                    break;
                                default:
                                    Debug.Log("erreur");
                                    break;
                            }
                            break;
                        case ItemType.Legs:
                            //Destroy(boots.gameObject);
                            break;
                        case ItemType.Chest:
                            //Destroy(chest.gameObject);
                            break;
                    }
                }
                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }
    public void OnAddItem(InventorySlot2 _slot)
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
                if (_slot.ItemObject.characterDisplay != null || _slot.ItemObject.characterDisplayRight != null || _slot.ItemObject.characterDisplayLeft != null )
                {
                    
                    switch (_slot.AllowedItems[0])
                    {
                        case ItemType.Helmet:
                            //helmetTransform = boneCombiner.AddLimb(_slot.ItemObject.characterDisplay,_slot.ItemObject.boneNames);
                            break;
                        case ItemType.Right:
                            Debug.Log("Right equip");
                            switch (_slot.ItemObject.type)
                            {
                                case ItemType.Weapon:
                                    weaponRightTransform = Instantiate(_slot.ItemObject.characterDisplayRight, weaponTransformRight).transform;
                                    SetAnimatorPlayer(_slot);
                                    break;
                                case ItemType.WeaponDouble:
                                    weaponRightTransform = Instantiate(_slot.ItemObject.characterDisplay, weaponTransformRight).transform;
                                    _slot.parent.inventory.GetSlots[3].AllowedItems[0] = ItemType.Disabled;
                                    inventory.AddItem(_slot.parent.inventory.GetSlots[3].item,1);
                                    //equipment.RemoveItem(_slot.parent.inventory.GetSlots[3].item);
                                    if (_slot.ItemObject.AnimatorOverrideBothHand != null)
                                    {
                                        Debug.Log("Double Hand");
                                        _animator.runtimeAnimatorController = _slot.ItemObject.AnimatorOverrideBothHand;
                                    }
                                    break;
                                default:
                                    Debug.Log("erreur");
                                    break;
                            }

                            break;
                        case ItemType.Left:
                            Debug.Log("Left equip");
                            switch (_slot.ItemObject.type)
                            {
                                case ItemType.Weapon:
                                    weaponLeftTransform = Instantiate(_slot.ItemObject.characterDisplayLeft, weaponTransformLeft).transform;
                                    SetAnimatorPlayer(_slot);
                                    break;
                                default:
                                    Debug.Log("erreur");
                                    break;
                            }
                            break;
                        case ItemType.Legs:
                            //bootsTransform = boneCombiner.AddLimb(_slot.ItemObject.characterDisplay, _slot.ItemObject.boneNames);
                            break;
                        case ItemType.Chest:
                            //chestTransform = boneCombiner.AddLimb(_slot.ItemObject.characterDisplay, _slot.ItemObject.boneNames);
                            break;
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


    public void SetAnimatorPlayer(InventorySlot2 _slot)
    {
        if (weaponRightTransform != null && weaponLeftTransform != null && _slot.ItemObject.AnimatorOverrideBothHand != null)
        {
            Debug.Log("Both Hand");
            _animator.runtimeAnimatorController = _slot.ItemObject.AnimatorOverrideBothHand;
        }
        else if (weaponRightTransform != null && weaponLeftTransform == null &&
                 _slot.ItemObject.AnimatorOverrideRightHand != null)
        {
            Debug.Log("Right Hand");
            _animator.runtimeAnimatorController = _slot.ItemObject.AnimatorOverrideRightHand;
        }
        else if (weaponRightTransform == null && weaponLeftTransform != null &&
                 _slot.ItemObject.AnimatorOverrideRightHand != null)
        {
            Debug.Log("Left Hand");
            _animator.runtimeAnimatorController = _slot.ItemObject.AnimatorOverrideLeftHand;
        }
        else {
            Debug.Log("Overide Animator default");
            _animator.runtimeAnimatorController = animatorOverrideDefault;
        }
    }
    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }

    public float CalculateDamage()
    {
        float alldamage = 0f;
        foreach (var slot in equipment.Container.Slots)
        {
            
            for (int i = 0; i < slot.AllowedItems.Length; i++)
            {
                if ((slot.AllowedItems[i] == ItemType.Weapon || slot.AllowedItems[i] == ItemType.WeaponDouble) && slot.item != null)
                {
                    alldamage = slot.item.valueFlat;
                }
            }
            
        }
        return alldamage;
    }
    
}

[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public Player parent;
    public StatTypes type;
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