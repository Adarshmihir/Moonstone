using System.Collections.Generic;
using Resources;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    //PREFABS SETTINGS
    [Header("Prefabs Settings")]
    public List<GameObject> objectToSpawn;

    [HideInInspector]
    public bool bDelay; //spawn every delay ?

    public bool isInDunguon; 

    public List<GameObject> saveObjectToSpawn;

    //SPAWNER SETTINGS
    private SphereCollider m_SpawnCollider;
    private Transform m_SpawnerTransform;

    [HideInInspector]
    public float spawnTime; //time to call spawn function in seconds 
    [HideInInspector]
    public float spawnDelay; //repeat rate of spawn function call after first call in seconds
    [HideInInspector]
    public float spawnerRadius; //defines the radius of the sphere collider of spawner
    [HideInInspector]
    public int maxArraySize; //maximum number of entities spawned

    private bool isSpawning = false;
    public bool stopSpawning = false; //stop spawning ?
    
    public List<GameObject> arrayObjectSpawned; //list of prefab instantiated 

    //draw method for spawner collider
    private void OnDrawGizmosSelected()
    {
        if (m_SpawnerTransform == null)
            m_SpawnerTransform = transform;
        Gizmos.color = new Color(0.58f, 0.50f, 0.16f, 1); //kaki
        Gizmos.DrawWireSphere(m_SpawnerTransform.position, spawnerRadius);
    }
    private void Start() {
        m_SpawnCollider = gameObject.AddComponent<SphereCollider>();
        m_SpawnCollider.radius = spawnerRadius;
        m_SpawnerTransform = GetComponent<Transform>();
        arrayObjectSpawned = new List<GameObject>();
        if (!bDelay)
        {
            for (int i = 0; i <= maxArraySize; ++i)
            {
                Invoke(nameof(SpawnObject), 0.1f);
            }
        }
        InvokeRepeating(nameof(SpawnObject), spawnTime, spawnDelay);
        isSpawning = true;
    }



    public void SpawnObject()
    {
        if (objectToSpawn.Count != 0)
        {
            var randObjectSpawn = this.GetRandomObjectToSpawn();
            var coordSpawn = this.GetRandomVector3Spawn();
            var objectSpawned = Instantiate(randObjectSpawn, coordSpawn, transform.rotation, this.transform);
            if (stopSpawning || arrayObjectSpawned.Count >= maxArraySize - 1)
            {
                CancelInvoke(nameof(SpawnObject));
                isSpawning = false;
            }
            objectSpawned.GetComponentInChildren<Health>().setSpawner(this);
            arrayObjectSpawned.Add(objectSpawned);
        }
    }



    private GameObject GetRandomObjectToSpawn()
    {
        var countObjects = objectToSpawn.Count;
        if (countObjects < 1) return null; //if list is null/empty return null

        if (countObjects == 1) //if there's only one object, spawn object
            return objectToSpawn[0];

        var value = Random.Range(0, countObjects); //spawn a random object in list
        return objectToSpawn[value];
    }

    private Vector3 GetRandomVector3Spawn()
    {
        var randPos = new Vector3(0, 0, 0);
       
        NavMeshHit hit;
        var bIsPosValid = true;
        while(bIsPosValid)
        {
            randPos = Random.insideUnitSphere * spawnerRadius;
            randPos += transform.position;


            if (NavMesh.SamplePosition(randPos, out hit, spawnerRadius, 1)) //only check walkable areas
            {
                randPos = hit.position;
                Debug.DrawRay(hit.position, Vector3.up, Color.blue, 1.0f);
                bIsPosValid = false;
            }
        }
        return randPos;
    }

    // Clear the arrayObjectSpawned and objectToSpawn and Destroy the GameObject in arrayObjectSpawned
    public void ClearSpawner()
    {
        for (var i = 0; i < arrayObjectSpawned.Count; i++)
        {
            Destroy(this.arrayObjectSpawned[i]);
        }
        arrayObjectSpawned.Clear();
        //objectToSpawn.Clear(); 
    }

    // Remove the element toRemove from arrayObjectSpawned 
    public void RemoveObject(GameObject toRemove)
    {
        arrayObjectSpawned.Remove(toRemove.transform.parent.gameObject);
        if (!isSpawning)
        {
            isSpawning = true;
            InvokeRepeating(nameof(SpawnObject), spawnDelay, spawnDelay);
        }
    }

    // Add a new element to spawn in the list objectToSpawn
    public void addObjectToSpawn(GameObject toAdd)
    {
        objectToSpawn.Add(toAdd);
    }
    // Remove an element to spawn in the list objectToSpawn
    public void removeObjectToSpawn(GameObject toRemove)
    {
        objectToSpawn.Remove(toRemove);
    }

    public void startPurge(List<GameObject> purgeObjectToSpawn)
    {
        if (!isInDunguon)
        {
            stopSpawning = true;
            saveObjectToSpawn = new List<GameObject>(objectToSpawn);
            ClearSpawner();
            objectToSpawn = new List<GameObject>(purgeObjectToSpawn);
            stopSpawning = false;
            InvokeRepeating(nameof(SpawnObject), spawnTime, spawnDelay);
        }
    }

    public void stopPurge()
    {
        if (!isInDunguon)
        {
            stopSpawning = true;
            ClearSpawner();
            objectToSpawn = new List<GameObject>(saveObjectToSpawn);
            saveObjectToSpawn = null;
            stopSpawning = false;
            InvokeRepeating(nameof(SpawnObject), spawnTime, spawnDelay);
        }
    }
}