using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopForgeron : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public ShopSlot[] slots;

    void Start()
    {
        slots = GetComponentsInChildren<ShopSlot>();

        for(int i=0; i < slots.Length;i++)
        {
            slots[i].AddItem(items[i]); 
        }
    }
}
