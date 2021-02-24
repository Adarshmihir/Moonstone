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
    public GameObject MiniMapCanvasGO;
    public GameObject CanvasSpellsBarGO;
    public GameObject StatsCanvasGO;
    public GameObject InventoryGO;
    public GameObject HealthGlobeGO;
    public GameObject CanvasRessourceGO;


    public void InitializeUIManager()
    {
        MiniMapCanvasGO = Instantiate(MiniMapCanvas);
        CanvasSpellsBarGO = Instantiate(CanvasSpellsBar);
        StatsCanvasGO = Instantiate(StatsCanvas);
        InventoryGO = Instantiate(Inventory);
        HealthGlobeGO = Instantiate(HealthGlobe);
        CanvasRessourceGO = Instantiate(CanvasRessource);
        //HideUIAtLaunch();
    }

    void HideUIAtLaunch()
    {
        StatsCanvasGO.SetActive(true);
        InventoryGO.SetActive(false);
    }
    
}
