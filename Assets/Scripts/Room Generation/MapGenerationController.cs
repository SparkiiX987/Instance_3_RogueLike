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
    private int journalIndex;
    
    private int roomIndex;
    private int transformQuestItem;
    
    private List<GameObject> transformItemsToDelete = new List<GameObject>();
    public List<GameObject> spawnedUsableItems = new List<GameObject>();
    
    private Save save => Save.Instance;

    [Header("Items")]
    [SerializeField] private List<GameObject> sellablesObjects;
    [SerializeField] private List<GameObject> usableObjects;
    [SerializeField] private GameObject journal;

    private bool didSpawnJournalItem;

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
            walkerScript.roomsGenerated[Random.Range(minRoomsBeforeSpawn, walkerScript.roomsGenerated.Count - 1)].transform
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
                        objectIndex = Random.Range(0, usableObjects.Count);
                        itemsSpwaned = Instantiate(usableObjects[objectIndex], transformItem.position, Quaternion.identity);
                        spawnedUsableItems.Add(itemsSpwaned);
                        
                        itemsSpwaned.transform.parent = itemsParent.transform;
                        Vector3 pos = itemsSpwaned.transform.localPosition;
                        itemsSpwaned.transform.localPosition = pos;
                    }
                    transformItemsToDelete.Add(transformItem.gameObject);
                }
            }
        }
        SpawnQuestItem();
        
        foreach (GameObject transformToDelete in transformItemsToDelete)
        {
            Destroy(transformToDelete);
        }
        SpawnJournal();
    }
    #endregion

    private void SpawnQuestItem()
    {
        objectIndex = save.GetCurrentQuest();
        for (int i = 0; i < questsList.Count; i++)
        {
            print(questsList[i].id);
            print(objectIndex);
            if (questsList[i].id == objectIndex)
            {
                roomIndex = Random.Range(1, rooms.Length);
                itemsParent = rooms[roomIndex].transform.GetChild(5);
                transformQuestItem = Random.Range(0, itemsParent.childCount);
                
                for (int j = 0; j < sellablesObjects.Count; j++)
                {
                    print(questsList[i].goalObject.name);
                    print(sellablesObjects[j].GetComponent<CollectableItem>().floorSprite.name);
                    if (questsList[i].goalObject.name == sellablesObjects[j].GetComponent<CollectableItem>().floorSprite.name) 
                    {
                        GameObject questItem = Instantiate(sellablesObjects[j], itemsParent.GetChild(transformQuestItem).position, Quaternion.identity);
                    }
                }
            }
        }
    }

    private void SpawnJournal()
    {
        if (didSpawnJournalItem == false)
        {
            journalIndex = Random.Range(0, spawnedUsableItems.Count);
            GameObject journalGo = Instantiate(journal, spawnedUsableItems[journalIndex].transform.position, Quaternion.identity);
            spawnedUsableItems[journalIndex].SetActive(false);
            didSpawnJournalItem = true;
        }
    }
}
