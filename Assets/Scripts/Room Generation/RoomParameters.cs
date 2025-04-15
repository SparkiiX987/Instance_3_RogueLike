using UnityEngine;

public class RoomParameters : MonoBehaviour
{
    //0->Top, 1->Left, 2->Right, 3->Down
    public int[] entraces;

    [SerializeField] private GameObject door;
    [SerializeField] private Transform[] doorways;
    [SerializeField, Range(0, 100)] private float doorSpawnChance;
    
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
}
