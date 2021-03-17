using Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootItem : MonoBehaviour
{
    [SerializeField] private Button icon;
    [SerializeField] private TextMeshProUGUI description;
    
    private Item _item;
    private CombatTarget _combatTarget;

    public void SetItem(Item newItem, CombatTarget combatTarget)
    {
        _item = newItem;
        icon.image.sprite = newItem.icon;
        description.text = newItem.name;
        _combatTarget = combatTarget;
        
        icon.onClick.AddListener(AddItem);
    }

    private void AddItem()
    {
        // TODO : Add item to inventory
        _combatTarget.DeleteItem(_item);
        Destroy(gameObject);
    }
}
