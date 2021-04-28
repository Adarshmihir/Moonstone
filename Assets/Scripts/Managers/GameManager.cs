using Combat;
using Core;
using ResourcesHealth;
using Stats;
using UI.Quests;
using UnityEngine;
using UnityEngine.AI;
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
    [SerializeField] private FeedbackMessage feedbackMessage;
    [SerializeField] private Animator loaderAnimator;
    [SerializeField] private GameObject boss;
    //PUBLIC VARIABLES (SHOWN IN INSPECTOR)
    public Transform PlayerSpawnPosition;
    public GameObject PlayerPrefab;
    public FollowCamera camera;
    public bool isPurgeActive = false;

    public RawImage WeaponSlot => weaponSlot;
    public RawImage ArmorSlot => armorSlot;
    public RawImage PetSlot => petSlot;
    public FeedbackMessage FeedbackMessage => feedbackMessage;
    public Animator LoaderAnimator => loaderAnimator;
    public GameObject Boss => boss;

    private GameManager() { }
    
    public static GameManager Instance { get; private set; }

    private void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            uiManager.InventoryGO.SetActive(!uiManager.InventoryGO.activeSelf);
        }

        
        if (Input.GetKeyDown(KeyCode.C))
        {
            uiManager.StatsCanvasGO.SetActive(!uiManager.StatsCanvasGO.activeSelf);
            uiManager.StatsCanvasGO.GetComponent<StatList>().PointsAvailable.SetActive(true);
            if (uiManager.StatsCanvasGO.GetComponent<StatList>().bLvlupactive)
            {
                Debug.Log(uiManager.StatsCanvasGO.GetComponent<StatList>().bLvlupactive);
                uiManager.StatsCanvasGO.GetComponent<StatList>().LevelUpStatButtons.SetActive(true);
            }
            
        }

        if (Input.GetButtonDown("PurgeMenu")) {
            uiManager.PurgeMenuGO.GetComponent<PurgeMenu>().Fade();
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            uiManager.QuestGO.gameObject.GetComponentInChildren<Button>().onClick.Invoke();
        }   
    }

    private void Awake() {
        equipementManager = gameObject.GetComponent<EquipmentManager>();
        uiManager = gameObject.GetComponent<UIManager>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        
        uiManager.InitializeUIManager();
        //PLAYER INITIALIZATION
        var transform1 = PlayerSpawnPosition.transform;
        var position = transform1.position;
        PlayerGO = Instantiate(PlayerPrefab, position, transform1.rotation);
        PlayerGO.name = "Thorrie";
        player = PlayerGO.GetComponent<Player>();
        player.InitializePlayer();
        PlayerGO.GetComponent<FighterSpell>().InitializeFighterSpell();
        camera.InitializeCamera();
        uiManager.HideUIAtLaunch();
        
        equipementManager.Initialize_EquipmentManager();
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
