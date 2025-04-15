using UnityEngine;

public class MapGenerationController : MonoBehaviour
{
    private LevelGeneratorWalker walkerScript;
    
    [SerializeField] Transform entitiesParent;
    [SerializeField] GameObject playerPrefab;
    private bool playerSpawned = false;
    
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
}
