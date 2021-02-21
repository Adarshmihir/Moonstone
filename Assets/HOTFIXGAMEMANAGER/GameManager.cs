using System;
using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public EquipmentManager equipementManager;
    public Player player;
    public UIManager uiManager;
    
 
    private GameManager() {
        
    }    
    public static GameManager Instance {
        get { return _instance; }
    }

    private void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

        uiManager.InitializeUIManager();
        player.InitializePlayer();
        equipementManager.Initialize_EquipmentManager();

    }
 
    // Add your game mananger members here
    public void Pause(bool paused) {
        
        
    }
}
