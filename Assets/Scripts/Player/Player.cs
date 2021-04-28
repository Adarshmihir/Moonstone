using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Control;
using ResourcesHealth;
using Stats;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    GameObject UILife;
    public float life;
    public float mana;
    GameObject UIMana;
    //public List<Stat> stats;
    public int level;
    public float BONUS_HEATH_PER_POINT = 5f;
    public float BONUS_CRIT_PER_POINT = 0.01f;
    public float BONUS_SPELL_PER_POINT = 1f;
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
    
    
    public Transform HandTransformRight;
    public Transform HandTransformLeft;


    private BoneCombiner boneCombiner;
    private Animator _animator;
    
    private void Start() {
        
        boneCombiner = new BoneCombiner(gameObject);
        
        for (int i = 0; i < equipment.GetSlots.Length; i++) {
            equipment.GetSlots[i].OnBeforeUpdate += OnRemoveItem;
            equipment.GetSlots[i].OnAfterUpdate += OnAddItem;
        }
        _animator = GetComponent<Animator>();
        
    }

    public static float bonushealth = 5f;
    // Update is called once per frame
    void Update() {
        // a dégager si possible  hors de l'update
        //StatTextUpdate();
        
    }

    public void InitializeStats() {
        StatList statList = GameManager.Instance.uiManager.StatsCanvasGO.GetComponent<StatList>();
        UILife = statList.getNumberGameObject("Health");
        UIMana = statList.getNumberGameObject("Ressources");
        for (int i = 0; i < attributes.Length; i++) {
            attributes[i].SetParent(this);
        }
        StatTextUpdate();

    }
    public void InitializePlayer()
    {
        level = 0;
        InitializeStats();
    }

    

    public void StatTextUpdate() {
        
        StatList statList = GameManager.Instance.uiManager.StatsCanvasGO.GetComponent<StatList>();

        foreach (var child in statList.GetComponentsInChildren<StatNumberUpdate>())
        {
            foreach (var stat in attributes)
            {
                if (child.type == stat.type)
                {
                    child.updateStatefield(stat.value.BaseValue, stat.value.ModifiedValue);
                }
                
            }
            
        }
        UILife.GetComponent<Text>().text = this.GetComponent<Health>().MaxHealthPoints.ToString();
        UIMana.GetComponent<Text>().text = GameObject.Find("EnergyGlobe").GetComponentInChildren<EnergyGlobeControl>().maxEnergy.ToString();
    }

    public void AddPointToStat(GameObject point_Number) {
       string name = EventSystem.current.currentSelectedGameObject.name;
       name = name.Replace("_Button", "");
        foreach (var stat in attributes) {
            if (stat.type.ToString() == name) {
                StatList statList = GameManager.Instance.uiManager.StatsCanvasGO.GetComponent<StatList>();
                stat.value.BaseValue+=1;

                if (stat.type == StatTypes.Stamina)
                {
                    GetComponent<Health>().addHealthPlayer(BONUS_HEATH_PER_POINT);
                    GameObject.Find("EnergyGlobe").GetComponentInChildren<EnergyGlobeControl>().addEnergyPlayer((int)BONUS_HEATH_PER_POINT);
                }

                statList.lvlup_Points -= 1;
                point_Number.GetComponent<Text>().text = statList.lvlup_Points.ToString();
                if (statList.lvlup_Points <= 0)
                {
                    statList.LevelUpStatButtons.SetActive(false);
                    statList.bLvlupactive = false;
                }
                StatTextUpdate();
            }
        }
    }

    public void ResetStat()
    {
        foreach (var stat in attributes)
        {
            if (stat.type == StatTypes.Stamina)
            {
                this.GetComponent<Health>().addHealthPlayer(-(stat.value.BaseValue) * BONUS_HEATH_PER_POINT);
                GameObject.Find("EnergyGlobe").GetComponentInChildren<EnergyGlobeControl>().addEnergyPlayer((int)(-(stat.value.BaseValue) * BONUS_HEATH_PER_POINT));
            }
            stat.value.BaseValue = 0;
        }

        GameManager.Instance.uiManager.StatsCanvasGO.GetComponent<StatList>().ToggleReset(level);
        StatTextUpdate();
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
        inventory.gold = inventory.initGold;
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
                        if (attributes[j].type == StatTypes.Stamina)
                        {
                            GetComponent<Health>().addHealthPlayer(-(BONUS_HEATH_PER_POINT*(attributes[j].value.ModifiedValue-attributes[j].value.BaseValue)));
                            GameObject.Find("EnergyGlobe").GetComponentInChildren<EnergyGlobeControl>().addEnergyPlayer((int)-(BONUS_HEATH_PER_POINT*(attributes[j].value.ModifiedValue-attributes[j].value.BaseValue)));
                        }
                    }
                }

                StatTextUpdate();
                if (_slot.ItemObject.characterDisplay != null || _slot.ItemObject.characterDisplayRight != null || _slot.ItemObject.characterDisplayLeft != null )
                {
                    switch (_slot.AllowedItems[0])
                    {
                        case ItemType.Helmet:
                            GameManager.Instance.player.GetComponent<FighterSpell>().UpdateSpell(null, CastSource.Pet);
                            //Destroy(helmet.gameObject);
                            break;
                        case ItemType.Weapon:
                            GameManager.Instance.player.GetComponent<FighterSpell>().UpdateSpell(null, CastSource.Weapon);
                            switch (_slot.ItemObject.type[1])
                            {
                                case ItemType.UniqueWeapon:
                                    Destroy(weaponRightTransform.gameObject);
                                    weaponRightTransform = null;
                                    weaponLeftTransform = null;
                                    SetAnimatorPlayer(_slot);
                                    break;
                                case ItemType.DualWeapon:
                                    Destroy(weaponRightTransform.gameObject);
                                    Destroy(weaponLeftTransform.gameObject);
                                    weaponRightTransform = null;
                                    weaponLeftTransform = null;
                                    SetAnimatorPlayer(_slot);
                                    break;
                                case ItemType.DoubleHandWeapon:
                                    Destroy(weaponRightTransform.gameObject);
                                    weaponRightTransform = null;
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
                            GameManager.Instance.player.GetComponent<FighterSpell>().UpdateSpell(null, CastSource.Armor);
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
                        {
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                            if (attributes[j].type == StatTypes.Stamina)
                            {
                                Debug.Log("oof v'la la stamina");
                                GetComponent<Health>().addHealthPlayer(BONUS_HEATH_PER_POINT *
                                                                       (attributes[j].value.ModifiedValue -
                                                                        attributes[j].value.BaseValue));
                                GameObject.Find("EnergyGlobe").GetComponentInChildren<EnergyGlobeControl>()
                                    .addEnergyPlayer((int) BONUS_HEATH_PER_POINT * (attributes[j].value.ModifiedValue -
                                        attributes[j].value.BaseValue));
                            }
                        }
                    }
                }

                StatTextUpdate();
                if (_slot.ItemObject.characterDisplay != null || _slot.ItemObject.characterDisplayRight != null || _slot.ItemObject.characterDisplayLeft != null )
                {
                    
                    switch (_slot.AllowedItems[0])
                    {
                        case ItemType.Helmet:
                            //helmetTransform = boneCombiner.AddLimb(_slot.ItemObject.characterDisplay,_slot.ItemObject.boneNames);
                            GameManager.Instance.player.GetComponent<FighterSpell>().UpdateSpell(_slot.ItemObject.Spell, CastSource.Pet);
                            
                            break;
                        case ItemType.Weapon:
                            GameManager.Instance.player.GetComponent<FighterSpell>().UpdateSpell(_slot.ItemObject.Spell, CastSource.Weapon);
                            switch (_slot.ItemObject.type[1])
                            {
                                case ItemType.UniqueWeapon:
                                    weaponRightTransform = Instantiate(_slot.ItemObject.characterDisplay, HandTransformRight).transform;
                                    SetAnimatorPlayer(_slot);
                                    break;
                                case ItemType.DualWeapon:
                                    weaponRightTransform = Instantiate(_slot.ItemObject.characterDisplayRight, HandTransformRight).transform;
                                    weaponLeftTransform = Instantiate(_slot.ItemObject.characterDisplayLeft, HandTransformLeft).transform;
                                    SetAnimatorPlayer(_slot);
                                    break;
                                case ItemType.DoubleHandWeapon:
                                    weaponRightTransform = Instantiate(_slot.ItemObject.characterDisplay, HandTransformRight).transform;
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
                            GameManager.Instance.player.GetComponent<FighterSpell>().UpdateSpell(_slot.ItemObject.Spell, CastSource.Armor);
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
        if ((weaponRightTransform != null || weaponLeftTransform != null) && _slot.ItemObject.AnimatorOverride != null)
        {
            _animator.runtimeAnimatorController = _slot.ItemObject.AnimatorOverride;
        }
       
        else {
            _animator.runtimeAnimatorController = animatorOverrideDefault;
        }
    }
    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }
// Ajouter degat lorsqu'on a rien d'équiper 
    public float CalculateDamage(Item2 CurrentItem = null)
    {
        float alldamage = 0f;
        
        if (CurrentItem != null)
        {
            var slot = equipment.FindItemOnInventory(CurrentItem);
            alldamage = slot.item.valueFlat;
            foreach (var stat in attributes)
            {
                if (stat.type == slot.item.damageBuffStat)
                {
                    alldamage += (stat.value.BaseValue + stat.value.ModifiedValue) *
                                 slot.item.damageBuffPercentValue;
                }
            }
        }
        else
        {
            alldamage = 5;
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
