using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
    public bool isPurgeActive = false;

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
            uiManager.EquipmentGO.SetActive(!uiManager.EquipmentGO.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.A))
            uiManager.StatsCanvasGO.SetActive(!uiManager.StatsCanvasGO.activeSelf);

        if (Input.GetButtonDown("PurgeMenu")) {
            uiManager.PurgeMenuGO.SetActive(!uiManager.PurgeMenuGO.activeSelf);
        }    
    }

    private void Awake() {
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
        uiManager.HideUIAtLaunch();
        
        equipementManager.Initialize_EquipmentManager();
    }

    private void Start() {
        
    }
    
    // Add your game mananger members here
    public void Pause(bool paused) {
        
        
    }
}
