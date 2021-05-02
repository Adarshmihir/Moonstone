using System;
using System.Collections.Generic;
using Combat;
using Core;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class LootBag : MonoBehaviour, IAction
{
    [SerializeField] private GameObject prefabItem;
    [SerializeField] private Button close;
    [SerializeField] private Transform contentParent;
    [SerializeField] private Sprite coin;

    public CombatTarget ActualTargetLoot;
    public List<ItemObject> ActualLootList;
    public bool IsLooting { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        IsLooting = false; 
        close.onClick.AddListener(CloseLootBag);
    }

    private void Update()
    {
        //Debug.Log(ActualTargetLoot);
    }

    public void InitLootbag(CombatTarget combatTarget)
    {
        ActualTargetLoot = combatTarget;
        ActualLootList = ActualTargetLoot.ListLoot;
        Debug.Log(ActualTargetLoot + "je suis le combat target");
        if (ActualLootList.Count > 0)
        {
            GameManager.Instance.uiManager.LootBagGO.SetActive(true);
            GameManager.Instance.uiManager.InventoryGO.SetActive(true);
            GetComponent<StaticInterface>().inventory.Clear();
            foreach (var loot in ActualLootList)
            {
                Debug.Log("init new loot");
                Debug.Log(loot);
                GetComponent<StaticInterface>().inventory.AddItem(new Item2(loot), 1);
            }
        }
    }
    
    

    public static bool IsLootBagOpen()
    {
        return GameManager.Instance.uiManager.LootBagGO.activeSelf;
    }


    public void Cancel()
    {
       
    }

    public void CloseLootBag()
    {
        IsLooting = false;
        var newLootList = new List<ItemObject>();
        Debug.Log( ActualTargetLoot);
        foreach (var slot in GetComponent<StaticInterface>().inventory.GetSlots)
        {
            if (slot.ItemObject != null)
            {
                newLootList.Add(slot.ItemObject);
            }
        }
        ActualTargetLoot.ListLoot = newLootList;
        
        GetComponent<StaticInterface>().inventory.Clear();
        GameManager.Instance.uiManager.LootBagGO.SetActive(false);
        GameManager.Instance.uiManager.InventoryGO.SetActive(false);
    }
}
