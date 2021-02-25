using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    //MANAGERS HIDE IN INSPECTOR
    [HideInInspector]
    public EquipmentManager equipementManager;
    [HideInInspector]
    public UIManager uiManager;
    
    //PUBLIC VARIABLES (SHOWN IN INSPECTOR)
    public Player player;
    private static GameManager _instance;
 
    private GameManager() {
        
    }    
    public static GameManager Instance {
        get { return _instance; }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            uiManager.InventoryGO.SetActive(!uiManager.InventoryGO.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            uiManager.StatsCanvasGO.SetActive(!uiManager.StatsCanvasGO.activeSelf);
        }
    }

    private void Start()
    {
        equipementManager = gameObject.GetComponent<EquipmentManager>();
        uiManager = gameObject.GetComponent<UIManager>();
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        uiManager.InitializeUIManager();
        player.InitializePlayer();
        equipementManager.Initialize_EquipmentManager();
        uiManager.HideUIAtLaunch();
    }
    
    // Add your game mananger members here
    public void Pause(bool paused) {
        
        
    }
}
