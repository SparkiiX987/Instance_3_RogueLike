using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RoomParameters : MonoBehaviour
{
    //0->Top, 1->Left, 2->Right, 3->Down
    public int[] entraces;
    [SerializeField] private Transform[] doorways;

    [FormerlySerializedAs("door")]
    [Header("Door Parameters")]
    [SerializeField] private GameObject doubleDoor;
    [SerializeField, Range(0, 100)] private float doorSpawnChance;
    [SerializeField] private bool canDoorSpawn;
    
    [Header("Wall Parameters")]
    [SerializeField] private GameObject wall;
    [SerializeField] private Transform wallParent;
    [SerializeField] private float raycastOffset;

    public void SpawnWallsCheck()
    {
        print("Spawning walls");
        foreach (int i in entraces)
        {
            switch (i)
            {
                case 0://up
                    if (!Physics.Raycast(transform.position + new Vector3(0, raycastOffset, 0), Vector3.forward, 1))
                    {
                        SpawnWalls(i);
                    }
                    else
                    {
                        
                    }
                    break;
                case 1://left
                    if (!Physics.Raycast(transform.position - new Vector3(raycastOffset, 0), Vector3.forward, 1))
                    {
                        SpawnWalls(i);
                    }
                    else
                    {
                        
                    }
                    break;
                case 2://right
                    if (!Physics.Raycast(transform.position + new Vector3(raycastOffset, 0), Vector3.forward, 1))
                    {
                        SpawnWalls(i);
                    }
                    else
                    {
                        
                    }
                    break;
                case 3://down
                    if (!Physics.Raycast(transform.position - new Vector3(0, raycastOffset), Vector3.forward, 1))
                    {
                        SpawnWalls(i);
                    }
                    else
                    {
                        
                    }
                    break;
            }
        }
        if (canDoorSpawn) TrySpawnDoors();
    }

    private void SpawnWalls(int index)
    {
        try
        {
            Instantiate(wall, doorways[index].position, doorways[index].rotation, wallParent);
        }
        catch { print("wrong room parameters"); }
    }
    
    private void TrySpawnDoors()
    {
        foreach (Transform t in doorways)
        {
            if (!(Random.Range(0, 100) <= doorSpawnChance)) continue;
            if (!Physics.Raycast(t.position, Vector3.forward, 1))
            {
                Instantiate(doubleDoor, t.position, t.rotation, this.transform);
            }
        }
    }
}
