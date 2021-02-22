using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject MiniMapCanvas;
    public GameObject CanvasSpellsBar;
    public GameObject StatsCanvas;
    public GameObject Inventory;
    public GameObject HealthGlobe;
    public GameObject CanvasRessource;
    public GameObject Enchantress;
    public GameObject MiniMapCanvasGO;
    public GameObject CanvasSpellsBarGO;
    public GameObject StatsCanvasGO;
    public GameObject InventoryGO;
    public GameObject HealthGlobeGO;
    public GameObject CanvasRessourceGO;
    public GameObject EnchantressGO;

    public void InitializeUIManager()
    {
        MiniMapCanvasGO = Instantiate(MiniMapCanvas);
        CanvasSpellsBarGO = Instantiate(CanvasSpellsBar);
        StatsCanvasGO = Instantiate(StatsCanvas);
        InventoryGO = Instantiate(Inventory);
        InventoryGO.GetComponent<InventoryUI>().Initialize_InventoryUI();
        EnchantressGO = Instantiate(Enchantress);
        EnchantressGO.GetComponent<EnchantressUI>().Initialize_EnchantressUI();
        HealthGlobeGO = Instantiate(HealthGlobe);
        CanvasRessourceGO = Instantiate(CanvasRessource);
    }

   public void HideUIAtLaunch()
    {
        StatsCanvasGO.SetActive(false);
        InventoryGO.SetActive(false);
        EnchantressGO.SetActive(false);
    }
    
}
