using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [Header("Entrance Settings")]
    [SerializeField] GameObject entrancePrefab;
    
    [Header("Room Settings")]
    [SerializeField] float moveDistance;
    [SerializeField] int MoveAmount;
    [SerializeField] int minMoveAmount;
    [SerializeField] int maxMoveAmount;
    public GameObject[] roomsArray;
    private int direction;
    
    
    void Start()
    {
        Instantiate(entrancePrefab, transform.position, Quaternion.identity);
        Move();
    }

    private void Move()
    {
        for (; MoveAmount > 0; MoveAmount -= Random.Range(minMoveAmount, maxMoveAmount))
        {
            
            direction = Random.Range(1, 5);
            switch (direction)
            {
                case 1:/*UP*/
                    print("up");
                    transform.position += new Vector3(moveDistance, 0, 0);
                    break;
                case 2:/*Left*/
                    print("left");
                    transform.position += new Vector3(0, moveDistance, 0);
                    break;
                case 3:/*Right*/
                    print("right");
                    transform.position -= new Vector3(moveDistance, 0, 0);
                    break;
                case 4:/*Down*/
                    print("down");
                    transform.position -= new Vector3(0, moveDistance, 0);
                    break;
            }
            Instantiate(roomsArray[0], transform.position, Quaternion.identity);
        }
    }
}
