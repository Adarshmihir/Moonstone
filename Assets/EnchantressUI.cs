using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnchantressUI : MonoBehaviour
{
    public InventoryEnchantressUI inventoryenchantress;
    public GameObject EnchantressMainSlotButton;
    public GameObject ModTextPrefab;
    public Transform ListMod;
    // Start is called before the first frame update
    public void Initialize_EnchantressUI()
    {
        inventoryenchantress.Initialize_InventoryEnchantressUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //Generate a modifier list when an Item is dropped on enchanteress UI
    public void GenerateModifierList(Item item)
    {
        if (!(item is Equipment eqItem)) return;
        foreach (var statModifier in eqItem.StatModifiers)
        {
            var go = Instantiate(ModTextPrefab, ListMod, true) as GameObject;
            go.GetComponent<Text>().text = " + " + statModifier.Value + " " + statModifier.statType + " ";
        }
    }

    public void ClearModifierList()
    { 
        List<Transform> ModifiersTextPrefabs = ListMod.Cast<Transform>().ToList();
        foreach (var ModifierGameObject in ModifiersTextPrefabs)
        {
            Destroy(ModifierGameObject.gameObject);
        }

    }
}
