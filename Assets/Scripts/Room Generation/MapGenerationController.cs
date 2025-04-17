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
    private RoomParameters[] rooms;
    float timer = 0.1f;
    bool done;

    [Header("Items")]
    [SerializeField] private List<GameObject> sellablesObjects;
    [SerializeField] private List<GameObject> usableObjects;

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
                rooms = FindObjectsOfType(typeof(RoomParameters)) as RoomParameters[];
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
        Transform itemsParent;
        Transform transformItem;
        GameObject itemsSpwaned;

        float chanceOfObject;
        float chanceOfTypeObject;
        int objectIndex;

        List<GameObject> transformItemsToDelete = new List<GameObject>();

        foreach (RoomParameters room in rooms)
        {
            itemsParent = room.transform.GetChild(room.transform.childCount - 1);
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

                        }
                        else
                        {
                            objectIndex = Random.Range(0, usableObjects.Count);
                            itemsSpwaned = Instantiate(usableObjects[objectIndex], transformItem.position, Quaternion.identity);
                        }
                        itemsSpwaned.transform.parent = itemsParent.transform;
                        Vector3 pos = itemsSpwaned.transform.localPosition;
                        itemsSpwaned.transform.localPosition = pos;
                    }
                    transformItemsToDelete.Add(transformItem.gameObject);
                }
            }
        }

        foreach (GameObject transformToDelete in transformItemsToDelete)
        {
            Destroy(transformToDelete);
        }
    }
    #endregion
}
