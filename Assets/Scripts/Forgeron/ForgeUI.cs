using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgeUI : MonoBehaviour
{
    private Inventory inventory;

    //A remplir manuellement
    public List<Combat.Weapon> weaponsAvailable;
    public Dropdown weaponList;

    public Text copper;
    public Text iron;
    public Text silver;

    public Button forgeBtn;

    public Image weaponImage;

    private Combat.Weapon weapon;

    private void Update()
    {
        UpateWeapon();
    }

    private void UpateWeapon()
    {
        int selectedWeaponsIndex = weaponList.value;
        string name = "Sword";
        if (weaponList.options.Count != 0)
        {
            name = weaponList.options[selectedWeaponsIndex].text;
        }
        
        foreach(Combat.Weapon item in weaponsAvailable)
        {
            if(item.name == name)
            {
                weapon = item;
            }
        }

        inventory = Inventory.instance;

        copper.text = "Copper required : " + inventory.copper + " / " + weapon.copperToBuild;
        iron.text = "Iron required : " + inventory.iron + " / " + weapon.ironToBuild;
        silver.text = "Silver required : " + inventory.silver + " / " + weapon.silverToBuild;

        if(inventory.copper >= weapon.copperToBuild && inventory.iron >= weapon.ironToBuild && inventory.silver >= weapon.silverToBuild)
        {
            forgeBtn.interactable = true;
        }

        weaponImage.sprite = weapon.icon;
    }

    public void onForge()
    {
        inventory = Inventory.instance;

        //inventory.Add(weapon); Weapon have to be an Item

        //Debug.Log("Weapon forged : " + weapon.weaponName);
    }
}
