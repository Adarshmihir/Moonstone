using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantressUI : MonoBehaviour
{
    public InventoryEnchantressUI inventoryenchantress;
    public GameObject EnchantressMainSlotButton;
    // Start is called before the first frame update
    public void Initialize_EnchantressUI()
    {
        inventoryenchantress.Initialize_InventoryEnchantressUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
