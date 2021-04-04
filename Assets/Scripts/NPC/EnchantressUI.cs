using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Quests;
using Combat;

public class EnchantressUI : MonoBehaviour
{
    public const float k_EnchantmentValueLimits = 10;
    public InventoryEnchantressUI inventoryenchantress;
    public GameObject EnchantressMainSlotButton;
    public GameObject ModTextPrefab;
    public GameObject FinishEnchantButton;
    public Transform ListMod;
    public Button selectedButton;
    private Item EquipmentToEnchant;
    private List<StatModifier> modifierList;
    Color selectedColor = Color.blue;
    Color unSelectedColor = Color.white;


    private float EnchantressDefaultPrice = 50f;




    // Start is called before the first frame update
    public void Initialize_EnchantressUI()
    {
        inventoryenchantress.Initialize_InventoryEnchantressUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (EnchantressMainSlotButton.GetComponent<EnchantressMainSlot>().icon.sprite == null)
        {
            FinishEnchantButton.GetComponent<Button>().enabled = false;
        }

        FinishEnchantButton.GetComponent<Button>().enabled = selectedButton != null;
    }

    //Generate a modifier list when an Item is dropped on enchanteress UI
    public void GenerateModifierList()
    {
        if (EquipmentToEnchant is Equipment)
        {
            modifierList = ((Equipment)EquipmentToEnchant).StatModifiers;
        }
        else
        {
            modifierList = ((Weapon)EquipmentToEnchant).StatModifiers;
        }
        
        foreach (var statModifier in modifierList)
        {
            var go = Instantiate(ModTextPrefab, ListMod, true) as GameObject;
            EnchantressModButton modButton = go.GetComponent<EnchantressModButton>();
            modButton.TextModButton.text = " + " + statModifier.Value + " " + statModifier.statType + " ";
            modButton.mod = statModifier;
        }
    }

    public void ClearModifierList()
    {
        selectedButton = null;
        List<Transform> ModifiersTextPrefabs = ListMod.Cast<Transform>().ToList();
        foreach (var ModifierGameObject in ModifiersTextPrefabs)
        {
            Destroy(ModifierGameObject.gameObject);
        }

    }

    public void SelectButton(Button modButton)
    {
        if (selectedButton != null)
        {
            selectedButton.GetComponent<Image>().color = unSelectedColor;
            selectedButton = modButton;
            selectedButton.GetComponent<Image>().color = selectedColor;
        }
        else
        {
            selectedButton = modButton;
            modButton.GetComponent<Image>().color = selectedColor;
        }

    }

    public void ValidateModifierChange()
    {
        Debug.Log("Valide Enchant !");
        //STAT MODIFICATION (REMOVING MOD + PUT NEW MOD)
        StatModifier ModSelected = selectedButton.GetComponent<EnchantressModButton>().mod;
        RemoveModBeingModified(ModSelected);
        StatModType randomModType = RandomEnum<StatModType>.Get();
        StatTypes randomStatType = RandomEnum<StatTypes>.Get();
        StatModifier ModModified = StatModifier.CreateInstance((float)System.Math.Round(Random.Range(ModSelected.Value - k_EnchantmentValueLimits, ModSelected.Value + k_EnchantmentValueLimits),1), randomModType, this, randomStatType);
        modifierList.Add(ModModified);
        //STAT MOD LIST CHANGE

        if (EquipmentToEnchant is Equipment)
        {
            ((Equipment)EquipmentToEnchant).StatModifiers = modifierList;
        }
        else
        {
            ((Weapon)EquipmentToEnchant).StatModifiers = modifierList;
        }

        EnchantressMainSlotButton.GetComponent<EnchantressMainSlot>().OnRemoveButton();

        Inventory.instance.gold -= EnchantressDefaultPrice;
		
        var player = GameObject.FindGameObjectWithTag("Player");
        var questList = player.GetComponent<QuestManager>();

        var evaluatedQuest = questList.Evaluate("HasQuest", "EnchantQuest");
        if (evaluatedQuest != null)
        {
            questList.CompleteGoal(questList.GetQuestByName("EnchantQuest"), "1");
        }
    }

    public void OnItemInMainSlot(Item item)
    {
        if (!(item is Equipment || item is Weapon)) return;
        EquipmentToEnchant = item;
        GenerateModifierList();

    }
    private void RemoveModBeingModified(StatModifier mod)
    {
        List<StatModifier> itemsToAdd = new List<StatModifier>();

        //only keep modifiers who won't be removed
        foreach (StatModifier modifier in modifierList) {
            if (modifier != mod) {
                itemsToAdd.Add(modifier);
            }
        }

        //update list
        modifierList = itemsToAdd;
    }


}
