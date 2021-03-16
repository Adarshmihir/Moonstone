using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //PREFABS
    public GameObject MiniMapCanvas;
    public GameObject CanvasSpellsBar;
    public GameObject StatsCanvas;
    public GameObject Inventory;
    public GameObject HealthGlobe;
    public GameObject CanvasRessource;
    public GameObject Enchantress;
    public GameObject Forgeron;
    public GameObject LevelManager;
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
    public GameObject HealthGlobeGO;
    [HideInInspector]
    public GameObject CanvasRessourceGO;
    [HideInInspector]
    public GameObject EnchantressGO;
    [HideInInspector]
    public GameObject ForgeronGO;
    [HideInInspector]
    public GameObject LevelManagerGO;

    public void InitializeUIManager()
    {
        //MiniMapCanvasGO = Instantiate(MiniMapCanvas);
        //CanvasSpellsBarGO = Instantiate(CanvasSpellsBar);
        StatsCanvasGO = Instantiate(StatsCanvas);
        InventoryGO = Instantiate(Inventory);
        InventoryGO.GetComponent<InventoryUI>().Initialize_InventoryUI();
        EnchantressGO = Instantiate(Enchantress);
        EnchantressGO.GetComponent<EnchantressUI>().Initialize_EnchantressUI();
        //HealthGlobeGO = Instantiate(HealthGlobe);
        //CanvasRessourceGO = Instantiate(CanvasRessource);
        ForgeronGO = Instantiate(Forgeron);
        LevelManagerGO = Instantiate(LevelManager);
        LevelManagerGO.GetComponent<LevelManager>().InitializeLevelManager();
    }

    public void HideUIAtLaunch()
    {
        StatsCanvasGO.SetActive(false);
        InventoryGO.SetActive(false);
        //MiniMapCanvasGO.SetActive(false);
        //HealthGlobeGO.SetActive(false);
        //CanvasRessourceGO.SetActive(false);
        EnchantressGO.SetActive(false);
        ForgeronGO.SetActive(false);
    }
}
