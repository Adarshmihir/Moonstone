using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Core;
using Resources;
using Stats;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
    //MANAGERS HIDE IN INSPECTOR
    [HideInInspector]
    public EquipmentManager equipementManager;
    [HideInInspector]
    public UIManager uiManager;
    [HideInInspector]
    public GameObject PlayerGO;
    [HideInInspector]
    public Player player;
    [SerializeField] private RawImage weaponSlot;
    [SerializeField] private RawImage armorSlot;
    [SerializeField] private RawImage petSlot;
    //PUBLIC VARIABLES (SHOWN IN INSPECTOR)
    public Transform PlayerSpawnPosition;
    public GameObject PlayerPrefab;
    public FollowCamera camera;
    private static GameManager _instance;
    public bool isPurgeActive = false;

    public RawImage WeaponSlot => weaponSlot;
    public RawImage ArmorSlot => armorSlot;
    public RawImage PetSlot => petSlot;

    private GameManager() {
        
    }    
    public static GameManager Instance {
        get { return _instance; }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Inventory"))
            uiManager.InventoryGO.SetActive(!uiManager.InventoryGO.activeSelf);
        
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
        //PLAYER INITIALIZATION
        var transform1 = PlayerSpawnPosition.transform;
        var position = transform1.position;
        PlayerGO = Instantiate(PlayerPrefab, position, transform1.rotation);
        player = PlayerGO.GetComponent<Player>();
        player.InitializePlayer();
        PlayerGO.GetComponent<FighterSpell>().InitializeFighterSpell();
        camera.InitializeCamera();
        uiManager.HideUIAtLaunch();
        
        equipementManager.Initialize_EquipmentManager();
    }

    private void Start() {
        
    }

    public void RespawnPlayer()
    {
        PlayerGO.GetComponent<NavMeshAgent>().Warp(PlayerSpawnPosition.position);
        Health PlayerHealth = PlayerGO.GetComponent<Health>();
        PlayerHealth.ResetLifePlayer();
        uiManager.DeathGO.SetActive(false);
    }
    
    // Add your game mananger members here
    public void Pause(bool paused) {
        
        
    }
}
