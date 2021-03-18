using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Quests;

public class ShopSlot : MonoBehaviour
{
    public Image icon;
    public Text priceText;
    public int price = 30;

    private Item item;

    private Inventory inventory;

    public void AddItem(Item item)
    {
        this.item = item;
        icon.enabled = true;
        icon.sprite = this.item.icon;
        priceText.text = ""+price;
    }

    public void onBuy()
    {
        inventory = Inventory.instance;
        if(inventory.gold >= price)
        {
            inventory.gold -= price;
            inventory.Add(item);
            Debug.Log("Item acheté : " + item.name);
            var player = GameObject.FindGameObjectWithTag("Player");
            var questList = player.GetComponent<QuestManager>();

            var evaluatedQuest = questList.Evaluate("HasQuest", "ForgeQuest");
            if (evaluatedQuest != null)
            {
                questList.CompleteGoal(questList.GetQuestByName("ForgeQuest"), "1");
            }
        }
        else
        {
            Debug.Log("Pas assez d'argent");
        }
    }
}
