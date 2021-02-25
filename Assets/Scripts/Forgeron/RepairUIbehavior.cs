using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairUIbehavior : MonoBehaviour
{
    public Image itemSelected;
    public Button nextItem;
    public Button repairBtn;
    public Text repairCost;

    private List<Item> listItem;
    private Inventory inventory;

    private int index = 0;

    private int cost = 50;

    private void Update()
    {
        inventory = Inventory.instance;
        listItem = inventory.GetList();
        if (listItem.Count > 0)
        {
            itemSelected.sprite = listItem[index].icon;
        }

        if (itemSelected.sprite != null)
        {
            repairBtn.interactable = true;

        }
        else
        {
            repairBtn.interactable = false;
        }
    }

    private void Awake()
    {
        inventory = Inventory.instance;
        listItem = inventory.GetList();
        if (listItem.Count > 0)
        {
            itemSelected.sprite = listItem[index].icon;
        }
    }

    public void onNextItem()
    {
        if(index >= listItem.Count-1)
        {
            index = 0;
        }
        else
        {
            index++;
        }

        if (listItem.Count > 0)
        {
            itemSelected.sprite = listItem[index].icon;
            repairCost.text = "Cost : " + cost;
        }
        
    }

    public void OnRepair()
    {
        inventory = Inventory.instance;
        if (inventory.gold >= cost)
        {
            inventory.gold -= cost;
            //Repair item
            Debug.Log("Item reparé : " + listItem[index].name);
        }
        else
        {
            Debug.Log("Pas assez d'argent");
        }
    }
}
