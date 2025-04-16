using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomParameters : MonoBehaviour
{
    //0->Top, 1->Left, 2->Right, 3->Down
    public int[] entraces;

    [SerializeField] private GameObject door;
    [SerializeField] private Transform[] doorways;
    [SerializeField, Range(0, 100)] private float doorSpawnChance;
    
    [SerializeField] private GameObject wall;
    [SerializeField] private Transform wallParent;
    [SerializeField] private float offset;
    
    private void Start()
    {
        if (doorways != null && door != null)
        {
            SpawnDoors();
        }
    }

    private void SpawnDoors()
    {
        foreach (Transform t in doorways)
        {
            if (!(Random.Range(0, 100) <= doorSpawnChance)) continue;
            if (!Physics.Raycast(t.position, Vector3.forward, 1))
            {
                Instantiate(door, t.position, t.rotation, this.transform);
            }
        }
    }

    private void SpawnWallsCheck()
    {
        foreach (int i in entraces)
        {
            switch (i)
            {
                case 0://up
                    if (Physics.Raycast(transform.position + new Vector3(0, offset, 0), Vector3.forward, 1))
                    {
                        SpawnWalls(i);
                    }
                    break;
                case 1://left
                    if (Physics.Raycast(transform.position + new Vector3(-offset, 0, 0), Vector3.forward, 1))
                    {
                        SpawnWalls(i);
                    }
                    break;
                case 2://right
                    if (Physics.Raycast(transform.position + new Vector3(offset, 0, 0), Vector3.forward, 1))
                    {
                        SpawnWalls(i);
                    }
                    break;
                case 3://down
                    if (Physics.Raycast(transform.position + new Vector3(0, -offset, 0), Vector3.forward, 1))
                    {
                        SpawnWalls(i);
                    }
                    break;
            }
        }
    }

    private void SpawnWalls(int index)
    {
        try
        {
            Instantiate(wall, doorways[index].position, doorways[index].rotation, wallParent);
        }
        catch {
            print("wrong room parameters");
        }
    }
}


