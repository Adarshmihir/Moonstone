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

    public void ShowLootBag(List<Item> items, CombatTarget combatTarget)
    {        
        GameManager.Instance.player.GetComponent<ActionScheduler>().StartAction(this);
        GameManager.Instance.uiManager.LootBagGO.SetActive(true);
        
        var parent = GameManager.Instance.uiManager.LootBagGO.GetComponent<LootBag>().contentParent;
        var newItem = Instantiate(prefabItem, parent);
        var gold = (int) Random.Range(combatTarget.MINGold, combatTarget.MAXGold);

        if (gold > 0)
        {
            newItem.GetComponent<LootItem>().SetGold(gold, coin);
        }

        foreach (var item in items)
        {
            parent = GameManager.Instance.uiManager.LootBagGO.GetComponent<LootBag>().contentParent;
            newItem = Instantiate(prefabItem, parent);
            newItem.GetComponent<LootItem>().SetItem(Object.Instantiate(item), combatTarget);
        }

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

    private static void CloseBag()
    {
        foreach (Transform item in GameManager.Instance.uiManager.LootBagGO.GetComponent<LootBag>().contentParent)
        {
            Destroy(item.gameObject);
        }
        GameManager.Instance.uiManager.LootBagGO.SetActive(false);
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
