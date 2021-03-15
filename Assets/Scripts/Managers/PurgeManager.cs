using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using Combat;
using UnityEditor.SceneManagement;
using UnityEngine;
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

    public GameObject toSpawnMob;
    public GameObject toSpawnMonster;

    public bool playerWasAgressive = false;

    // Start is called before the first frame update
    void Start() {
        dungeonPos = GameObject.FindGameObjectWithTag("Dungeon").transform.position;
        purgeUI = GameManager.Instance.uiManager.PurgeMenuGO.GetComponent<PurgeMenu>();
        purgeUI.fill.fillAmount = 0;
        purgiumAmount = Math.Floor(purgeUI.fill.fillAmount * 100);
        
        purgeFillDurationInSeconds = DefaultPurgeFillDurationInSeconds;
        purgeIncrement = DefaultPurgeIncrement;

        numberToKill = purgeLevel * 2;
    }

    // Update is called once per frame
    void Update() {
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
                Debug.Log("Inside dungeon");
                // Increase purge bar faster
                purgeFillDurationInSeconds = 150;

                // If player kills a boss
                if (player.hasKilledABoss) {
                    // Increase purge bar by 5 purgium
                    purgeUI.fill.fillAmount += 0.05f;
                    player.hasKilledABoss = false;
                }
            } else {
                Debug.Log("Outside dungeon");
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

            // Force to open purge menu at beginning
            purgeUI.gameObject.SetActive(true);

            numberToKill = purgeLevel * 2;
            Debug.Log("Player need to kill " + numberToKill + " monsters to end purge");

            // Deny access to any structure expect village while purge is active
            //TODO: Voir corentin pour checker comme lui l'entree dans le portail.

            // Feedbacks
            // Sirene au declenchement into musique de combat
            //TODO: Peut etre appeler une methode de l'AudioManager ?

            // Make sure player is out of a dungeon
            if (!player.isInDungeon) {
                // Spawn monsters at each outside spawner //TODO: manage inside/outside spawners
                clearAndSetSpawners("Enemy_monster");

                // At purge end ...
                if (purgeUI.timer.timeRemaining <= 0) {
                    // If player killed all monsters (fixed number)
                    if (killedCount >= numberToKill) {
                        Debug.Log("All monsters killed !");
                        purgeUI.fill.fillAmount = 0;

                        // Spawn normal enemies
                        clearAndSetSpawners("Enemy_mob");
                    }
                    else { // Player didnt kill all monsters ..
                        // If player was aggressive (trying to complete purge) --> // isPlayerAggressive > (method : +1 to a counter when player hits a enemy. if counter > 0.75 * numberToKill = true
                        if (playerWasAgressive)
                            // TODO: Choisir une option avec le groupe (Voir GDD)
                            outOfTimePenalty();
                        else
                            lackOfKillsPenalty();
                    }
                } else if (killedCount >= numberToKill) {
                    // TODO: reset to normal mode
                }
            }
        }
        else {
            GameManager.Instance.isPurgeActive = false;
            killedCount = 0;
        }
            
    }
    private void SpawnOutOfDungeon(GameObject prefabToSpawn, Transform spawnpoint, int _numberToSpawn) {
        /*dungeonMonsters = getAllMonstersInDungeon();
             
        foreach (var monster in dungeonMonsters) {
            //Randomly spawn monsters in an area around player
            

            Instantiate(monster.transform.parent, new Vector3(randX, -10, randZ), new Quaternion(), spawnpoint);
            monster.transform.parent.position = new Vector3(randX, -10, randZ);
            Destroy(monster.transform.parent.gameObject);
        }*/
        
        

        for (int i = 0; i < _numberToSpawn; i++) {
            float randX = randomX.Next((int) spawnpoint.position.x - 10,
                                        (int) spawnpoint.position.x + 10);

            float randZ = randomZ.Next((int) spawnpoint.transform.position.z - 10,
                                        (int) spawnpoint.transform.position.z + 10);
            Instantiate(prefabToSpawn, new Vector3(randX, -10, randZ), new Quaternion(), spawnpoint.transform);
        }
    }

    public void clearAndSetSpawners(string tag) {
        foreach (Spawner _spawner in spawners) {
            if (_spawner.arrayObjectSpawned.Count > 0 && _spawner.arrayObjectSpawned[0].CompareTag("Enemy_mob")) {
                _spawner.ClearSpawner();
                Debug.Log(tag + " cleared!");

                switch (tag) {
                    case "Enemy_mob":
                        if(toSpawnMob != null) 
                            _spawner.addObjectToSpawn(toSpawnMob);
                        
                        GameManager.Instance.isPurgeActive = false;
                        break;
                    
                    case "Enemy_monster":
                        if(toSpawnMonster != null) 
                            _spawner.addObjectToSpawn(toSpawnMonster);
                        
                        
                        GameManager.Instance.isPurgeActive = true;
                        break;
                    
                    default:
                        Debug.Log("The tag you trying to instantiate doesn't exist");
                        break;
                }
            }
        }
    }

    public void outOfTimePenalty() {
        Debug.Log("loose cause of time");
    }

    public void lackOfKillsPenalty() {
        Debug.Log("loose cause you were lazy");
    }

    public bool checkPlayerAgressiveness() {


        return true;
    }
}
