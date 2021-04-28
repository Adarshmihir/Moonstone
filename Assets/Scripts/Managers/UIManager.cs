using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //PREFABS
    public GameObject MiniMapCanvas;
    public GameObject CanvasSpellsBar;
    public GameObject StatsCanvas;
    public GameObject Inventory;
    public GameObject Equipment;
    public GameObject HealthGlobe;
    public GameObject EnergyGlobe;
    public GameObject CanvasRessource;
    public GameObject Enchantress;
    public GameObject Forgeron;
    public GameObject LootBag;
    public GameObject LevelManager;
    public GameObject PurgeMenu;
    public GameObject Death;

    //GAMEOBJECTS
    [HideInInspector]
    public GameObject MiniMapCanvasGO;
    [HideInInspector]
    public GameObject CanvasSpellsBarGO;
    [HideInInspector]
    public GameObject StatsCanvasGO;
    [HideInInspector]
    public GameObject InventoryGO;
    [HideInInspector]
    public GameObject EquipmentGO;
    [HideInInspector]
    public GameObject HealthGlobeGO;
    [HideInInspector]
    public GameObject EnergyGlobeGO;
    [HideInInspector]
    public GameObject CanvasRessourceGO;
    [HideInInspector]
    public GameObject EnchantressGO;
    [HideInInspector]
    public GameObject ForgeronGO;
    [HideInInspector]
    public GameObject LootBagGO;
	[HideInInspector]
    public GameObject LevelManagerGO;
    [HideInInspector]
    public GameObject PurgeMenuGO;
    [HideInInspector]
    public GameObject DeathGO;
    public GameObject QuestGO;

    public void InitializeUIManager()
    {
        //MiniMapCanvasGO = Instantiate(MiniMapCanvas);
        //CanvasSpellsBarGO = Instantiate(CanvasSpellsBar);
        StatsCanvasGO = Instantiate(StatsCanvas);
        InventoryGO = Instantiate(Inventory);
        GetComponent<Inventory>().InitSingleton();
        InventoryGO.GetComponent<InventoryUI>().Initialize_InventoryUI();
        EquipmentGO = Instantiate(Equipment);
        EnchantressGO = Instantiate(Enchantress);
        EnchantressGO.GetComponent<EnchantressUI>().Initialize_EnchantressUI();
        //HealthGlobeGO = Instantiate(HealthGlobe);
        CanvasRessourceGO = Instantiate(CanvasRessource);
        ForgeronGO = Instantiate(Forgeron);
        LootBagGO = Instantiate(LootBag);
        LevelManagerGO = Instantiate(LevelManager);
        LevelManagerGO.GetComponent<LevelManager>().InitializeLevelManager();
        PurgeMenuGO = Instantiate(PurgeMenu);
        DeathGO = Instantiate(Death);
    }

    public void HideUIAtLaunch() {
        StatsCanvasGO.SetActive(false);
        InventoryGO.SetActive(false);
        EquipmentGO.SetActive(false);
        //MiniMapCanvasGO.SetActive(false);
        //HealthGlobeGO.SetActive(false);
        //CanvasRessourceGO.SetActive(false);
        EnchantressGO.SetActive(false);
        ForgeronGO.SetActive(false);
        LootBagGO.SetActive(false);
        PurgeMenuGO.SetActive(true);
        DeathGO.SetActive(false);
    }

    public void HideInventory()
    {
        InventoryGO.SetActive(!InventoryGO.activeSelf);
    }
    
    public void HideStats()
    {
        StatsCanvasGO.SetActive(!StatsCanvasGO.activeSelf);
    }
    
    
}
