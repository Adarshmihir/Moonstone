using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using Quests;
using Combat;
using JetBrains.Annotations;
using UnityEditor.UIElements;
using Random = UnityEngine.Random;

public class EnchantressUI : MonoBehaviour {
    public const float k_EnchantmentValueLimits = 10;

    public InventoryEnchantressUI inventoryenchantress;

    // public GameObject EnchantressMainSlotButton;
    // public GameObject ModTextPrefab;
    public GameObject FinishEnchantButton;

    public Transform ListMod;

    // public Button selectedButton;
    private Item EquipmentToEnchant;
    private InventorySlot2 toEnchant;
    private List<ItemBuff> modifierList = new List<ItemBuff>();
    Color selectedColor = Color.blue;
    Color unSelectedColor = Color.white;

    private float EnchantressDefaultPrice = 50f;

    // temp

    public InventoryObject inventory;
    public GameObject EnchantressMainSlotButton;
    public GameObject ModTextPrefab;
    public Button selectedButton;
    public Transform statContainer;
    public Font font;
    public InventorySlot2 enchantressSlot;


    // Start is called before the first frame update
    public void Initialize_EnchantressUI() {
        inventoryenchantress.Initialize_InventoryEnchantressUI();
    }

    private void Start() {
        inventory.GetSlots[0].OnBeforeUpdate += OnBeforeSlotUpdate;
        inventory.GetSlots[0].OnAfterUpdate += OnAfterSlotUpdate;
        enchantressSlot = inventory.GetSlots[0];
    }

    //Generate a modifier list when an Item is dropped on enchanteress UI
    public void GenerateModifierList(InventorySlot2 _slot) {
        foreach (var buff in _slot.item.buffs) {
            var go = Instantiate(ModTextPrefab, statContainer, true);
            EnchantressModButton modButton = go.GetComponent<EnchantressModButton>();
            modButton.TextModButton.text = buff.attribute + " : " + buff.value;
            modButton.buff = buff;
        }
    }

    public void createStatsDisplay(ItemBuff[] buffs) {
        foreach (var buff in buffs) {
            var go = Instantiate(ModTextPrefab, statContainer, true);
            EnchantressModButton modButton = go.GetComponent<EnchantressModButton>();
            modButton.TextModButton.text = buff.attribute + " : " + buff.value;
            modButton.buff = buff;
        }
    }

    public void ClearModifierList() {
        selectedButton = null;
        List<Transform> ModifiersTextPrefabs = ListMod.Cast<Transform>().ToList();
        foreach (var ModifierGameObject in ModifiersTextPrefabs) {
            Destroy(ModifierGameObject.gameObject);
        }
    }

    public void SelectButton(Button modButton) {
        if (selectedButton != null) {
            selectedButton.GetComponent<Image>().color = unSelectedColor;
            selectedButton = modButton;
            selectedButton.GetComponent<Image>().color = selectedColor;
        }
        else {
            selectedButton = modButton;
            modButton.GetComponent<Image>().color = selectedColor;
        }
    }

    private void RemoveModBeingModified(ItemBuff mod) {
        List<ItemBuff> itemsToAdd = new List<ItemBuff>();

        //only keep modifiers who won't be removed
        foreach (ItemBuff modifier in modifierList) {
            if (modifier != mod) {
                itemsToAdd.Add(modifier);
            }
        }

        //update list
        modifierList = itemsToAdd;
    }

    public void CloseMenu() {
        if (!GameManager.Instance.uiManager.EnchantressGO.activeSelf) return;

        Debug.Log(enchantressSlot.ItemObject);

        GameManager.Instance.uiManager.EnchantressGO.SetActive(false);
        // GameManager.Instance.uiManager.EnchantressGO.GetComponent<EnchantressUI>().EnchantressMainSlotButton.GetComponent<EnchantressMainSlot>().OnRemoveButton();
        if (enchantressSlot.ItemObject != null) {
            GameManager.Instance.player.inventory.AddItem(enchantressSlot.item, enchantressSlot.amount);
        }

        inventory.Container.Clear();
    }

    public void OnBeforeSlotUpdate(InventorySlot2 _slot) {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type) {
            case InterfaceType.Inventory:
                Debug.Log("interacting with inventory : before");
                break;
            case InterfaceType.Equipment:

                break;
            case InterfaceType.Chest:
                break;

            case InterfaceType.Enchantress:
                Debug.Log("interacting with enchantress : before");
                var btns = statContainer.GetComponentsInChildren<EnchantressModButton>();
                foreach (var btn in btns)
                    Destroy(btn.gameObject);

                break;
            default:
                break;
        }
    }

    public void OnAfterSlotUpdate(InventorySlot2 _slot) {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type) {
            case InterfaceType.Inventory:
                Debug.Log("interacting with inventory : after");
                break;
            case InterfaceType.Equipment:

                break;
            case InterfaceType.Chest:
                break;

            case InterfaceType.Enchantress:
                Debug.Log("interacting with enchantress : after");
                GenerateModifierList(_slot);

                break;
            default:
                break;
        }
    }

    public void ResetStat() {
        if (selectedButton) {
            ItemBuff[] buffs = enchantressSlot.item.buffs;
            List<ItemBuff> newBuffs = new List<ItemBuff>();

            ItemBuff selectedBuff = selectedButton.GetComponent<EnchantressModButton>().buff;
            var initAttr = selectedBuff.attribute;
            var initVal = selectedBuff.value;
            Debug.Log(initAttr + " : " + initVal);
            ItemBuff modifiedBuff = new ItemBuff(selectedBuff.min, selectedBuff.max);
            modifiedBuff.attribute = (StatTypes) Enum.ToObject(typeof(StatTypes), Random.Range(0, 4));
            modifiedBuff.value = Random.Range(modifiedBuff.min, modifiedBuff.max);


            Debug.Log("Modified : " + modifiedBuff.attribute + " : " + modifiedBuff.value);
            // Debug.Log("Initbuff values : " + initBuff.attribute + " : " + initBuff.value);

            #region STAT UPDATE

            if (selectedBuff.attribute == modifiedBuff.attribute) {
                selectedBuff.value = modifiedBuff.value;
                Debug.Log("You enchanted : " + enchantressSlot.item.Name + ".\n" + initAttr + " was " +
                          initVal + " and is now " + modifiedBuff.value);
            }
            else {
                var existing = Array.Find(buffs, buff => buff.attribute == modifiedBuff.attribute);

                if (existing == null) {
                    selectedBuff.attribute = modifiedBuff.attribute;
                    selectedBuff.value = modifiedBuff.value;
                    Debug.Log("You enchanted : " + enchantressSlot.item.Name + ".\n" + initAttr + " was " +
                              initVal + " but a magic thing happened !! " + initAttr +
                              " has changed to " + modifiedBuff.attribute + " with a new value of " +
                              modifiedBuff.value);
                }
                // if there is a mod that has the new mod's attribute in the list
                else {
                    // /!\ Makes a copy of buff list
                    newBuffs = buffs.ToList();
                    newBuffs.Remove(selectedBuff);

                    var existingInitVal = existing.value;
                    existing.value += modifiedBuff.value;


                    buffs = newBuffs.ToArray();

                    var amountAdded = modifiedBuff.value;
                    var finalVal = existingInitVal + amountAdded;

                    Debug.Log("You enchanted : " + enchantressSlot.item.Name + ".\n" + initAttr + " was " +
                              initVal + " but a magic thing happened !! " + initAttr +
                              " disappeared but " + modifiedBuff.attribute + " has been upgraded to " +
                               + finalVal +
                              " from " + existingInitVal + " (+" + /*modifiedBuff.value*/amountAdded + ")");
                }
            }

            // Display updated stat
            updateBuffDisplay();

            #endregion

            // Clear all stats
            foreach (var btn in statContainer.GetComponentsInChildren<EnchantressModButton>()) {
                Destroy(btn.gameObject);
            }
            
            // ISA 15 17 21

            // Display updated item stats
            displayStats(buffs);

            GameManager.Instance.player.inventory.gold -= EnchantressDefaultPrice;

            var player = GameObject.FindGameObjectWithTag("Player");
            // var player = GameManager.Instance.player;
            var questList = player.GetComponent<QuestManager>();

            var evaluatedQuest = questList.Evaluate("HasQuest", "EnchantQuest");
            if (evaluatedQuest != null) {
                questList.CompleteGoal(questList.GetQuestByName("EnchantQuest"), "1");
            }
        }
    }

    public void updateBuffDisplay() {
        var modbutton = selectedButton.GetComponent<EnchantressModButton>();

        // for (int i = 0; i < buffs.Length; i++) {
        //     modButtons[i].buff.attribute = buffs[i].attribute;
        //     modButtons[i].buff.value = buffs[i].value;
        // }
        // EnchantressModButton modBtn = selectedButton.GetComponent<EnchantressModButton>();
        //
        // modBtn.TextModButton.text = modBtn.buff.attribute + " : " + modBtn.buff.value;

        modbutton.TextModButton.text = modbutton.buff.attribute + " : " + modbutton.buff.value;
    }

    public void displayStats(ItemBuff[] buffs) {
        createStatsDisplay(buffs);
    }
}