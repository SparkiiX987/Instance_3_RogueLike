using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGeneratorWalker : MonoBehaviour
{
    #region Global Variables
    
    [Header("Global Variables (info only)")]
    public float roomsGenerated;

    #endregion

    #region Entrance Settings

    [Header("Entrance Settings")]
    [SerializeField] GameObject[] entrancePrefabs;
    [SerializeField] private Transform[] topEntranceSpawns;
    [SerializeField] private Transform[] leftEntranceSpawns;
    [SerializeField] private Transform[] rightEntranceSpawns;
    [SerializeField] private Transform[] bottomEntranceSpawns;

    #endregion

    #region Room Settings
    
    [Header("Room Settings")]
    [SerializeField] float maxPosX;
    [SerializeField] float maxPosY;
    [SerializeField] int maxRooms;
    [SerializeField] int MoveAmount;
    [SerializeField] int minMoveAmount;
    [SerializeField] int maxMoveAmount;
    [SerializeField] float moveDistance;
    [SerializeField] GameObject[] roomsArray;
    private int direction;
    
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
        int amount = Random.Range(minMoveAmount, maxMoveAmount);
        for (; MoveAmount > 0; MoveAmount -= amount)
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, 0, maxPosX),
                Mathf.Clamp(transform.position.y, 0, maxPosY), transform.position.z);
            amount = Random.Range(minMoveAmount, maxMoveAmount);
            if (roomsGenerated >= maxRooms)
            {
                return;
            }
            direction = Random.Range(0, 4);
            switch (direction)
            {
                case 0:/*UP*/
                    //print("up " + transform.position);
                    if (transform.position.y <= maxPosY) {
                        transform.position += new Vector3(moveDistance, 0, 0);
                        RoomGenerator(amount);
                    }
                    break;
                case 1:/*Left*/
                    //print("left " + transform.position);
                    if (transform.position.x >= 0) {
                        transform.position += new Vector3(0, moveDistance, 0);
                        RoomGenerator(amount);
                    }
                    break;
                case 2:/*Right*/
                    //print("right " + transform.position);
                    if (transform.position.x <= maxPosX) {
                        transform.position -= new Vector3(moveDistance, 0, 0);
                        RoomGenerator(amount);
                    }
                    break;
                case 3:/*Down*/
                    //print("down " + transform.position);
                    if (transform.position.y >= 0) {
                        transform.position -= new Vector3(0, moveDistance, 0);
                        RoomGenerator(amount);
                    }
                    break;
            }
        }
    }

    private void RoomGenerator(int amount)
    {
        if(!Physics.Raycast(transform.position, Vector3.forward, 1))
        {
            Instantiate(roomsArray[0], transform.position, Quaternion.identity);
            roomsGenerated += 1;
        }
        else
        {
            MoveAmount += amount;
        }
    }
}
