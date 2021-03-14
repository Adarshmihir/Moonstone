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

    public List<GameObject> dungeonMonsters = new List<GameObject>();
    public GameObject[] mobs;
    
    readonly Random randomX = new Random();
    readonly Random randomZ = new Random();
    
    [SerializeField]
    private GameObject[] spawnpoints;

    public int numberToSpawn = 0;

    public int killedCount = 0;
    public int numberToKill = 1;

    public GameObject monsterToSpawn;
    public GameObject mobToSpawn;

    // Start is called before the first frame update
    void Start() {
        dungeonPos = GameObject.FindGameObjectWithTag("Dungeon").transform.position;
        purgeUI = GameManager.Instance.uiManager.PurgeMenuGO.GetComponent<PurgeMenu>();
        purgeUI.fill.fillAmount = 0;
        purgiumAmount = Math.Floor(purgeUI.fill.fillAmount * 100);
        
        purgeFillDurationInSeconds = DefaultPurgeFillDurationInSeconds;
        purgeIncrement = DefaultPurgeIncrement;

        //set timer
        purgeUI.timer.timeRemaining = 1200;
    }

    // Update is called once per frame
    void Update() {
        GameManager.Instance.isPurgeActive = purgiumAmount >= 100;
        purgiumAmount = Math.Floor(purgeUI.fill.fillAmount * 100);
        purgeUI.fill.fillAmount += (purgeIncrement * Time.deltaTime) / purgeFillDurationInSeconds;
        
        if (!GameManager.Instance.isPurgeActive && PurgeManager.Instance.killedCount >= PurgeManager.Instance.numberToKill)
            purgeUI.fill.fillAmount = 0f;
        
        //spawnpoints = GameObject.FindGameObjectWithTag("Respawn").GetComponentsInChildren<Spawner>();
        spawnpoints = GameObject.FindGameObjectsWithTag("Respawn");
        
        

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
         if (GameManager.Instance.isPurgeActive) {
             // Deny access to any structure expect village while purge is active
             //TODO: Voir corentin pour checker comme lui l'entree dans le portail.
             
             // Feedbacks
             // Sirene au declenchement into musique de combat
             //TODO: Peut etre appeler une methode de l'AudioManager ?
             
             // Make sure player is out of a dungeon
             if (!player.isInDungeon) {
                 // Spawn "n" monsters outside of dungeon at each spawpoint, "n" being defined by purge lvl
                 foreach (GameObject spawnpoint in spawnpoints) {
                     numberToSpawn = 3;
                     if(spawnpoint.transform.childCount < numberToSpawn)
                         SpawnOutOfDungeon(monsterToSpawn, spawnpoint.transform, numberToSpawn);
                 }

                 // Vanish all mobs (= open world ennemies)
                 if (getAllMobs().Length > 0) {
                     DestroyAllMobs();
                 }
             }
             
             // Player didnt kill all monsters ..
             else {
                 // Because he ran out of time
             }
             // Else
             // TODO: Choisir une option avec le groupe (Voir GDD)
         }
         
         // If player kills all monsters (fixed number)
         if (killedCount >= numberToKill) {
             purgeUI.fill.fillAmount = 0;
                 
             // Spawn normal enemies
             foreach (GameObject spawnpoint in spawnpoints) {
                 numberToSpawn = 1;
                 if(spawnpoint.transform.childCount < numberToSpawn)
                     SpawnOutOfDungeon(mobToSpawn, spawnpoint.transform, numberToSpawn);
             }
             
             // End of purge
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
    
    private GameObject[] getAllMobs() { 
        return GameObject.FindGameObjectsWithTag("Enemy_mob");
    }
    
    /*private List<GameObject> getAllMonstersInDungeon() {
        List<GameObject> monsters = new List<GameObject>();
        
        // Store all enemies in dungeon
        Collider[] colliders = Physics.OverlapSphere(dungeonPos, collider.radius, LayerMask.GetMask("Enemy_monster"));
        foreach (Collider col in colliders) {
            monsters.Add(col.gameObject);
        }

        return monsters;
    }*/
    
    private List<GameObject> getAllMonstersOutside() {
        List<GameObject> monsters = new List<GameObject>();
        
        // Store all enemies in dungeon
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy_monster");
        
        foreach (GameObject go in temp) {
            monsters.Add(go);
        }

        return monsters;
    }

    private void DestroyAllMobs() {
        mobs = getAllMobs();
        foreach (var mob in mobs) {
            Destroy(mob.transform.parent.gameObject);
        }
    }
}
