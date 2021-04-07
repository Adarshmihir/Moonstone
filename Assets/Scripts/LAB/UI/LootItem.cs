using System;
using Combat;
using TMPro;
using UI.Quests;
using UnityEngine;
using UnityEngine.UI;

public class LootItem : MonoBehaviour
{
    [SerializeField] private Button icon;
    [SerializeField] private TextMeshProUGUI description;

    private CombatTarget _combatTarget;

    public Item Item { get; private set; }

    public void SetItem(Item newItem, CombatTarget combatTarget)
    {
        Item = newItem;
        icon.image.sprite = newItem.icon;
        description.text = newItem.name;
        _combatTarget = combatTarget;
        
        icon.onClick.AddListener(AddItem);
    }

    public void SetGold(int amount)
    {
        // TODO : Update gold image
        icon.image.sprite = UnityEngine.Resources.Load<Sprite>("Sprites/CloseButton");
        description.text = amount.ToString();

        GetComponent<TooltipSpawner>().enabled = false;
        
        icon.onClick.AddListener(AddGold);
    }

    private void AddItem()
    {
        GetComponent<TooltipSpawner>().HideTooltip();
        Inventory.instance.Add(Item);
        _combatTarget.DeleteItem(Item);
        Destroy(gameObject);
    }

    private void AddGold()
    {
        Inventory.instance.gold += int.Parse(description.text);
        Destroy(gameObject);
    }
}
