using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [Header("Global Variables")]
    [HideInInspector] public float roomsGenerated;
    
    [Header("Entrance Settings")]
    [SerializeField] GameObject entrancePrefab;
    [SerializeField] private Transform[] topEntranceSpawns;
    [SerializeField] private Transform[] leftEntranceSpawns;
    [SerializeField] private Transform[] rightEntranceSpawns;
    [SerializeField] private Transform[] bottomEntranceSpawns;
    
    [Header("Room Settings")]
    [SerializeField] float moveDistance;
    [SerializeField] int MoveAmount;
    [SerializeField] int minMoveAmount;
    [SerializeField] int maxMoveAmount;
    [SerializeField] GameObject[] roomsArray;
    private int direction;
    
    
    void Start()
    {
        switch (Random.Range(1, 5))
        {
            case 1:
                
                break;
        }
        Instantiate(entrancePrefab, transform.position, Quaternion.identity);
        roomsGenerated += 1;
        Move();
    }

    private void Move()
    {
        for (; MoveAmount > 0; MoveAmount -= Random.Range(minMoveAmount, maxMoveAmount))
        {
            if (roomsGenerated >= 256)
            {
                return;
            }
            direction = Random.Range(1, 5);
            switch (direction)
            {
                case 1:/*UP*/
                    transform.position += new Vector3(moveDistance, 0, 0);
                    break;
                case 2:/*Left*/
                    transform.position += new Vector3(0, moveDistance, 0);
                    break;
                case 3:/*Right*/
                    transform.position -= new Vector3(moveDistance, 0, 0);
                    break;
                case 4:/*Down*/
                    transform.position -= new Vector3(0, moveDistance, 0);
                    break;
            }
            roomsGenerated += 1;
            Instantiate(roomsArray[0], transform.position, Quaternion.identity);
        }
    }
}
