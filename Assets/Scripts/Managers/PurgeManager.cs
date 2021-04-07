using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using Combat;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = System.Random;

public class PurgeManager : MonoBehaviour {
    #region Singleton
    public static PurgeManager Instance;
    private void Awake() {
        if (Instance != null) Debug.LogWarning("More than one instance of PurgeManager found !");
        Instance = this;
    }
    #endregion

    public Player player;

    private new SphereCollider collider;
    private Vector3 dungeonPos;
    private Vector3 zoneCenter;

    public PurgeMenu purgeUI;
    private int purgeFillDurationInSeconds;
    private const int DefaultPurgeFillDurationInSeconds = 300;
    private int purgeIncrement;
    private const int DefaultPurgeIncrement = 1;
    public double purgiumAmount = 0;
    
    readonly Random randomX = new Random();
    readonly Random randomZ = new Random();
    
    [SerializeField]
    private Spawner[] spawners;
    public int killedCount = 1;
    public int numberToKill = 0;

    public int purgeLevel = 1;

    public List<GameObject> objectToSpawn;
    private bool isSpawnsSet;

    public bool playerWasAgressive = false;

    public Animator screenEffectAnimator;

    // Start is called before the first frame update
    private void Start() {
        dungeonPos = GameObject.FindGameObjectWithTag("Dungeon").transform.position;
        purgeUI = GameManager.Instance.uiManager.PurgeMenuGO.GetComponent<PurgeMenu>();
        purgeUI.fill.fillAmount = 0;
        purgiumAmount = Math.Floor(purgeUI.fill.fillAmount * 100);
        
        purgeFillDurationInSeconds = DefaultPurgeFillDurationInSeconds;
        purgeIncrement = DefaultPurgeIncrement;

        numberToKill = purgeLevel * 2;

        isSpawnsSet = false;
    }

    // Update is called once per frame
    private void Update() {
        purgiumAmount = Math.Floor(purgeUI.fill.fillAmount * 100);
        purgeUI.fill.fillAmount += (purgeIncrement * Time.deltaTime) / purgeFillDurationInSeconds;

        // if (GameManager.Instance.isPurgeActive && killedCount >= CountMonsters())
        //     purgeUI.fill.fillAmount = 0f;
        
        //spawners = GameObject.FindGameObjectWithTag("Respawn").GetComponentsInChildren<Spawner>();
        spawners = GameObject.FindObjectsOfType<Spawner>();
        
        player = GameManager.Instance.player;
        collider = GameObject.FindGameObjectWithTag("Dungeon").GetComponent<SphereCollider>();
        
        // Collider[].Length > 0 means the Player collides with the "Dungeon Zone" collider area
        player.isInDungeon = Physics.OverlapSphere(dungeonPos, collider.radius, LayerMask.GetMask("Player")).Length > 0;

        // 1.Check if player is in a dungeon // 2. Check if player has killed a boss
        #region DungeonCheck
            if (player.isInDungeon) {
                //Debug.Log("Inside dungeon");
                // Increase purge bar faster
                purgeFillDurationInSeconds = 150;

                // If player kills a boss
                if (player.hasKilledABoss) {
                    // Increase purge bar by 5 purgium
                    purgeUI.fill.fillAmount += 0.05f;
                    player.hasKilledABoss = false;
                }
            } else {
               // Debug.Log("Outside dungeon");
                // Increase purge bar at normal rate
                purgeIncrement = DefaultPurgeIncrement;
                purgeFillDurationInSeconds = DefaultPurgeFillDurationInSeconds;

                player.isInDungeon = false;
            }
        #endregion

        // Trigger the purge mode
        // If purge bar is full
        if (purgiumAmount >= 100) {
            GameManager.Instance.isPurgeActive = true;
            screenEffectAnimator.SetBool("isPurgeActive", true);

            // Force to open purge menu at beginning
            purgeUI.gameObject.SetActive(true);

            numberToKill = purgeLevel * 2;

            // Deny access to any structure expect village while purge is active
            //TODO: Voir corentin pour checker comme lui l'entree dans le portail.

            // Make sure player is out of a dungeon
            if (!player.isInDungeon) {
                // Spawn monsters at each outside spawner //TODO: manage inside/outside spawners
                if(!isSpawnsSet)
                {
                    foreach (Spawner spawner in spawners)
                    {
                        spawner.startPurge(objectToSpawn);
                    }
                    isSpawnsSet = true;
                }
                
                // If player killed all monsters (fixed number)
                if (killedCount >= numberToKill)
                {
                    ResetNormalMode();
                }

                // At purge end ...
                if (purgeUI.GetComponentInChildren<Timer>().timeRemaining <= 0) {
                    
                    // If player killed all monsters (fixed number)
                    if (killedCount < numberToKill) { // Player didnt kill all monsters ..
                        // If player was aggressive (trying to complete purge) --> // isPlayerAggressive > (method : +1 to a counter when player hits a enemy. if counter > 0.75 * numberToKill = true
                        checkPlayerAgressiveness();
                        if (playerWasAgressive)
                            // TODO: Choisir une option avec le groupe (Voir GDD)
                            outOfTimePenalty();
                        else
                            lackOfKillsPenalty();
                    }
                    ResetNormalMode();
                    purgeUI.fill.fillAmount = 0;
                } else if (killedCount >= numberToKill) 
                    ResetNormalMode();
            }
        }
        else {
            GameManager.Instance.isPurgeActive = false;
            killedCount = 0;
            screenEffectAnimator.SetBool("isPurgeActive", false);
        } 
    }


    private void outOfTimePenalty() {
        Debug.Log("Purge is over ! You tried nice but unfortunately you didnt kill all enemies in time. A time penalty of TO DECIDE seconds. will be applied to next purge");
    }

    private void lackOfKillsPenalty() {
        Debug.Log("Purge is over ! You weren't aggressive enough. A time penalty of TO DECIDE seconds. will be applied to the next purge");
    }

    private void checkPlayerAgressiveness() {
        // logic about player agressiveness
        playerWasAgressive = true;
    }

    private void ResetNormalMode() {
        purgeUI.fill.fillAmount = 0;
        killedCount = 0;
        
        /*
        // Spawn normal enemies
        clearAndSetSpawners("Enemy_mob");
        */

        foreach (Spawner spawner in spawners)
        {
            spawner.stopPurge();
        }
        isSpawnsSet = false;
        GameManager.Instance.isPurgeActive = false;

        ComputePurgePenalty();
    }

    private void ComputePurgePenalty() {
        
    }
}
