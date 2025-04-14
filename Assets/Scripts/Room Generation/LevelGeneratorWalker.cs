using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGeneratorWalker : MonoBehaviour
{
    #region Global Variables
    
    [Header("Global Variables (info only)")]
    public int roomsGenerated;
    public int cyclesPassed;
    public int[] currentEntraces;

    #endregion

    #region Entrance Settings

    [Header("Entrance Settings")]
    [SerializeField] GameObject[] entrancePrefabs;
    [SerializeField] private Transform[] topEntranceSpawns;
    [SerializeField] private Transform[] leftEntranceSpawns;
    [SerializeField] private Transform[] rightEntranceSpawns;
    [SerializeField] private Transform[] bottomEntranceSpawns;

    #endregion

    #region Room Generation Settings

    [Header("Room Generation Settings")] 
    [SerializeField] int maxCycles;
    [SerializeField] int maxRooms;
    [SerializeField] float maxPosX;
    [SerializeField] float maxPosY;
    [SerializeField] int MoveAmount;
    [SerializeField] int minMoveAmount;
    [SerializeField] int maxMoveAmount;
    [SerializeField] float moveDistance;
    private int randomEntrance;
    private RaycastHit Hit;
    
    #endregion

    #region Rooms

    [Header("Rooms")]
    [SerializeField] GameObject[] tRoomPrefabs;
    [SerializeField] GameObject[] lRoomPrefabs;
    [SerializeField] GameObject[] rRoomPrefabs;
    [SerializeField] GameObject[] bRoomPrefabs;

    #endregion
    
    
    
    void Start()
    {
        SetEntranceSpawn();
        WalkerMovment();
    }

    private void SetEntranceSpawn()
    {
        int selectedPrefab = Random.Range(0, 4);
        switch (selectedPrefab)
        {
            case 0:/*UP*/
                transform.position = topEntranceSpawns[Random.Range(0, topEntranceSpawns.Length)].position;
                break;
            case 1:/*Left*/
                transform.position = leftEntranceSpawns[Random.Range(0, leftEntranceSpawns.Length)].position;
                break;
            case 2:/*Right*/
                transform.position = rightEntranceSpawns[Random.Range(0, rightEntranceSpawns.Length)].position;
                break;
            case 3:/*Down*/
                transform.position = bottomEntranceSpawns[Random.Range(0, bottomEntranceSpawns.Length)].position;
                break;
        }
        Instantiate(entrancePrefabs[selectedPrefab], transform.position, Quaternion.identity);
        roomsGenerated += 1;
    }
    
    private void WalkerMovment()
    {
        Physics.Raycast(transform.position, Vector3.forward, out Hit, 1);
        currentEntraces = Hit.collider.gameObject.GetComponent<RoomParameters>().entraces;

        int amount = Random.Range(minMoveAmount, maxMoveAmount);
        for (; MoveAmount > 0; MoveAmount -= amount)
        {
            
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, 0, maxPosX),
                Mathf.Clamp(transform.position.y, 0, maxPosY), transform.position.z);
            amount = Random.Range(minMoveAmount, maxMoveAmount);
            
            Physics.Raycast(transform.position, Vector3.forward, out Hit, 1);
            currentEntraces = Hit.collider.gameObject.GetComponent<RoomParameters>().entraces;
            
            cyclesPassed++;

            if (cyclesPassed >= maxCycles)
            {
                return;
            }

            if (roomsGenerated >= maxRooms)
            {
                return;
            }
            
            randomEntrance = Random.Range(0, currentEntraces.Length);
            switch (currentEntraces[randomEntrance])
            {
                case 0:/*UP*/
                    if (transform.position.y <= maxPosY) {
                        transform.position += new Vector3(0, moveDistance, 0);
                        RoomGenerator(amount, 0);
                    }
                    break;
                case 1:/*Left*/
                    if (transform.position.x >= 0) {
                        transform.position -= new Vector3(moveDistance, 0, 0);
                        RoomGenerator(amount, 1);
                    }
                    break;
                case 2:/*Right*/
                    if (transform.position.x <= maxPosX) {
                        transform.position += new Vector3(moveDistance, 0, 0);
                        RoomGenerator(amount, 2);
                    }
                    break;
                case 3:/*Down*/
                    if (transform.position.y >= 0) {
                        transform.position -= new Vector3(0, moveDistance, 0);
                        RoomGenerator(amount, 3);
                    }
                    break;
            }
        }
    }

   

    private void RoomGenerator(int amount, int entrance)
    {
        if(!Physics.Raycast(transform.position, Vector3.forward, out Hit, 1))
        {
            switch (entrance)
            {
                case 0://Summon down room
                    Instantiate(bRoomPrefabs[Random.Range(0, bRoomPrefabs.Length)], transform.position, Quaternion.identity);
                    break;
                case 1://Summon right room
                    Instantiate(rRoomPrefabs[Random.Range(0, rRoomPrefabs.Length)], transform.position, Quaternion.identity);
                    break;
                case 2://Summon left room
                    Instantiate(lRoomPrefabs[Random.Range(0, lRoomPrefabs.Length)], transform.position, Quaternion.identity);
                    break;
                case 3://Summon up room
                    Instantiate(tRoomPrefabs[Random.Range(0, tRoomPrefabs.Length)], transform.position, Quaternion.identity);
                    break;
            }
            roomsGenerated += 1;
        }
        else
        {
            MoveAmount += amount;
        }
    }
}
