using System;
using System.Collections.Generic;
using Combat;
using Core;
using UnityEngine;
using UnityEngine.UI;

public class LootBag : MonoBehaviour
{
    [SerializeField] private GameObject prefabItem;
    [SerializeField] private Button close;
    [SerializeField] private Transform contentParent;

    // Start is called before the first frame update
    private void Start()
    {
        close.onClick.AddListener(CloseBag);
    }

    public void ShowLootBag(List<Item> items, CombatTarget combatTarget)
    {
        if (items.Count == 0) return;
        
        GameManager.Instance.uiManager.LootBagGO.SetActive(true);
        foreach (var item in items)
        {
            var parent = GameManager.Instance.uiManager.LootBagGO.GetComponent<LootBag>().contentParent;
            var newItem = Instantiate(prefabItem, parent);
            newItem.GetComponent<LootItem>().SetItem(item, combatTarget);
        }
    }

    private void CloseBag()
    {
        foreach (Transform item in GameManager.Instance.uiManager.LootBagGO.GetComponent<LootBag>().contentParent)
        {
            Destroy(item.gameObject);
        }
        gameObject.SetActive(false);
    }
}
