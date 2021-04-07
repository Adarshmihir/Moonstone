using System.Collections.Generic;
using Combat;
using Core;
using UnityEngine;
using UnityEngine.UI;

public class LootBag : MonoBehaviour, IAction
{
    [SerializeField] private GameObject prefabItem;
    [SerializeField] private Button close;
    [SerializeField] private Transform contentParent;
    
    public bool IsLooting { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        IsLooting = false;
        close.onClick.AddListener(Cancel);
    }

    public void ShowLootBag(List<Item> items, CombatTarget combatTarget)
    {
        if (items.Count == 0) return;
        
        GameManager.Instance.player.GetComponent<ActionScheduler>().StartAction(this);
        GameManager.Instance.uiManager.LootBagGO.SetActive(true);
        
        var parent = GameManager.Instance.uiManager.LootBagGO.GetComponent<LootBag>().contentParent;
        var newItem = Instantiate(prefabItem, parent);
        var gold = (int) Random.Range(combatTarget.MINGold, combatTarget.MAXGold);

        if (gold > 0)
        {
            newItem.GetComponent<LootItem>().SetGold(gold);
        }

        foreach (var item in items)
        {
            parent = GameManager.Instance.uiManager.LootBagGO.GetComponent<LootBag>().contentParent;
            newItem = Instantiate(prefabItem, parent);
            newItem.GetComponent<LootItem>().SetItem(item, combatTarget);
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
        IsLooting = false;
        CloseBag();
    }
}
