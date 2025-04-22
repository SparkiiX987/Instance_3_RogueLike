using System.Collections.Generic;
using UnityEngine;

public class MapGenerationController : MonoBehaviour
{
    private LevelGeneratorWalker walkerScript;

    [SerializeField] Transform entitiesParent;
    [SerializeField] GameObject playerPrefab;
    private bool playerSpawned = false;

    #region Spawn Items Variables
    [Header("Rooms")]
    
    public List<QuestScriptableObject> questsList = new List<QuestScriptableObject>();
    
    private RoomParameters[] rooms;
    float timer = 0.1f;
    bool done;
    
    private Transform itemsParent;
    private Transform transformItem;
    private GameObject itemsSpwaned;

    private float chanceOfObject;
    private float chanceOfTypeObject;
    private int objectIndex;
    private int objectQuestIndex;
    private int journalIndex;
    
    private List<GameObject> transformItemsToDelete = new List<GameObject>();
    public List<GameObject> spawnedItems = new List<GameObject>();
    
    private Save save => Save.Instance;

    [Header("Items")]
    [SerializeField] private List<GameObject> sellablesObjects;
    [SerializeField] private List<GameObject> usableObjects;
    [SerializeField] private GameObject journal;

    [Header("Percentage")]
    [SerializeField] private float percentageOfObject;
    [SerializeField] private float percentageOfType;
    #endregion

    #region Monster Spawn Settings

    [Header("Monster Spawn Settings")]
    [SerializeField] GameObject monsterPrefab;
    [SerializeField] int minRoomsBeforeSpawn;

    #endregion

    private void Start()
    {
        walkerScript = GetComponentInChildren<LevelGeneratorWalker>();
        
        GameObject playerControl = (sellablesObjects[0]);
        playerControl.AddComponent<Save>();
        objectQuestIndex = playerControl.GetComponent<Save>().GetCurrentQuest();
    }

    private void Update()
    {
        SpawnPlayer();
        if (!done)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                rooms = FindObjectsByType(typeof(RoomParameters), FindObjectsSortMode.None) as RoomParameters[];
                GenerateItems();
                done = true;
            }
        }
    }

    private void SpawnPlayer()
    {
        if (!walkerScript.finished || playerSpawned) return;
        Instantiate(playerPrefab, walkerScript.roomsGenerated[0].transform.position, Quaternion.identity, entitiesParent);
        playerSpawned = true;
        SpawnMonster();
    }

    private void SpawnMonster()
    {
        Instantiate(monsterPrefab,
            walkerScript.roomsGenerated[Random.Range(minRoomsBeforeSpawn, walkerScript.roomsGenerated.Count)].transform
                .position, Quaternion.identity, entitiesParent);
    }


    #region Spawn Items
    private void GenerateItems()
    {
        foreach (RoomParameters room in rooms)
        {
            itemsParent = room.transform.GetChild(5);
            int limit = itemsParent.childCount;
            for (int i = 0; i < limit; i++)
            {
                transformItem = itemsParent.GetChild(i).GetComponent<Transform>();
                if (transformItem != null)
                {
                    chanceOfObject = Random.Range(0f, 1f);
                    if (chanceOfObject > percentageOfObject)
                    {
                        chanceOfTypeObject = Random.Range(0f, 1f);
                        if (chanceOfTypeObject > percentageOfType)
                        {
                            objectIndex = Random.Range(0, sellablesObjects.Count);
                            itemsSpwaned = Instantiate(sellablesObjects[objectIndex], transformItem.position, Quaternion.identity);
                            spawnedItems.Add(itemsSpwaned);
                        }
                        else
                        {
                            objectIndex = Random.Range(0, usableObjects.Count);
                            itemsSpwaned = Instantiate(usableObjects[objectIndex], transformItem.position, Quaternion.identity);
                            spawnedItems.Add(itemsSpwaned);
                        }
                        itemsSpwaned.transform.parent = itemsParent.transform;
                        Vector3 pos = itemsSpwaned.transform.localPosition;
                        itemsSpwaned.transform.localPosition = pos;
                    }
                    transformItemsToDelete.Add(transformItem.gameObject);
                }
            }
        }

        /*for (int i = 0; i < questsList.Count ; i++)
        {
            journalIndex = Random.Range(0, spawnedItems.Count);
            if (objectQuestIndex == questsList[i].id)
            {
                
                if (spawnedItems[journalIndex].GetComponent<Sprite>().name != questsList[i].goalObject.name)
                {
                    itemsSpwaned = Instantiate(journal, spawnedItems[journalIndex].transform.position, Quaternion.identity);
                    Destroy(spawnedItems[journalIndex]);
                    break;
                }
            }
        }*/

        foreach (GameObject transformToDelete in transformItemsToDelete)
        {
            Destroy(transformToDelete);
        }
    }
    #endregion
}
