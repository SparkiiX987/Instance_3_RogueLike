using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGeneratorWalker : MonoBehaviour
{
    #region Global Variables

    [field:Header("Global Variables (info only)")]
    [field:SerializeField] public int cyclesPassed{ get; private set;}
    [field:SerializeField] public List<GameObject> roomsGenerated{ get; private set;}
    public bool finished{ get; private set;}
    [HideInInspector] public int[] currentEntraces;
    

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
    [SerializeField] Transform roomsParent;
    private int randomEntrance;
    private RaycastHit hit;
    
    #endregion

    #region Rooms

    [Header("Rooms")]
    [SerializeField] GameObject[] tRoomPrefabs;
    [SerializeField] GameObject[] lRoomPrefabs;
    [SerializeField] GameObject[] rRoomPrefabs;
    [SerializeField] GameObject[] bRoomPrefabs;

    #endregion

    [Header("graph")]
    [SerializeField] private Transform graph;
    
    void Start()
    {
        finished = false;
        SetEntranceSpawn();
        WalkerMovment();
    }

    private void SetEntranceSpawn()
    {
        int _selectedPrefab = Random.Range(0, 4);
        switch (_selectedPrefab)
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
        roomsGenerated.Add(Instantiate(entrancePrefabs[_selectedPrefab], transform.position, Quaternion.identity, roomsParent));
    }
    
    private void WalkerMovment()
    {
        Physics.Raycast(transform.position, Vector3.forward, out hit, 1);
        currentEntraces = hit.collider.gameObject.GetComponent<RoomParameters>().entraces;

        int _amount = Random.Range(minMoveAmount, maxMoveAmount);
        for (; MoveAmount > 0; MoveAmount -= _amount)
        {
            
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, 0, maxPosX),
                Mathf.Clamp(transform.position.y, 0, maxPosY), transform.position.z);
            _amount = Random.Range(minMoveAmount, maxMoveAmount);
            
            Physics.Raycast(transform.position, Vector3.forward, out hit, 1);
            print(hit);
            print(hit.collider);
            print(hit.collider.gameObject.name);
            currentEntraces = hit.collider.GetComponent<RoomParameters>().entraces;
            
            cyclesPassed++;

            if (cyclesPassed >= maxCycles)
            {
                RegroupNodes();
                finished = true;
                return;
            }

            if (roomsGenerated.Count >= maxRooms)
            {
                RegroupNodes();
                finished = true;
                return;
            }
            
            randomEntrance = Random.Range(0, currentEntraces.Length);
            switch (currentEntraces[randomEntrance])
            {
                case 0:/*UP*/
                    if (transform.position.y <= maxPosY) {
                        transform.position += new Vector3(0, moveDistance, 0);
                        RoomGenerator(_amount, 0);
                    }
                    break;
                case 1:/*Left*/
                    if (transform.position.x >= 0) {
                        transform.position -= new Vector3(moveDistance, 0, 0);
                        RoomGenerator(_amount, 1);
                    }
                    break;
                case 2:/*Right*/
                    if (transform.position.x <= maxPosX) {
                        transform.position += new Vector3(moveDistance, 0, 0);
                        RoomGenerator(_amount, 2);
                    }
                    break;
                case 3:/*Down*/
                    if (transform.position.y >= 0) {
                        transform.position -= new Vector3(0, moveDistance, 0);
                        RoomGenerator(_amount, 3);
                    }
                    break;
            }
        }
        RegroupNodes();
        finished = true;
    }

   private void RegroupNodes()
   {
        print(roomsGenerated.Count);
        List<Transform> nodes = new List<Transform>();
        for(int i = 0; i < roomsGenerated.Count; i++)
        {
            Node[] nodesInRoom = roomsGenerated[i].transform.GetChild(4).GetComponentsInChildren<Node>();
            print(roomsGenerated[i].transform.GetChild(4).name);
            foreach(Node node in nodesInRoom)
            {
                nodes.Add(node.transform);
            }
        }
        for(int y = 0; y < nodes.Count; ++y)
        {
            nodes[y].SetParent(graph);
        }
   }

    private void RoomGenerator(int _amount, int _entrance)
    {
        if(!Physics.Raycast(transform.position, Vector3.forward, out hit, 1))
        {
            switch (_entrance)
            {
                case 0://Summon down room
                    roomsGenerated.Add(Instantiate(bRoomPrefabs[Random.Range(0, bRoomPrefabs.Length)], transform.position, Quaternion.identity, roomsParent));
                    break;
                case 1://Summon right room
                    roomsGenerated.Add(Instantiate(rRoomPrefabs[Random.Range(0, rRoomPrefabs.Length)], transform.position, Quaternion.identity, roomsParent));
                    break;
                case 2://Summon left room
                    roomsGenerated.Add(Instantiate(lRoomPrefabs[Random.Range(0, lRoomPrefabs.Length)], transform.position, Quaternion.identity, roomsParent));
                    break;
                case 3://Summon up room
                    roomsGenerated.Add(Instantiate(tRoomPrefabs[Random.Range(0, tRoomPrefabs.Length)], transform.position, Quaternion.identity, roomsParent));
                    break;
            }
        }
        else
        {
            MoveAmount += _amount;
        }
    }
}
