using System.Collections.Generic;
using UnityEngine;

public class GeneratorItems : MonoBehaviour
{
    [Header("Rooms")]
    private RoomParameters[] rooms;
    float timer = 2f;
    bool done;

    [Header("Items")]
    [SerializeField] private List<CollectableItem> sellablesObjects;
    [SerializeField] private List<CollectableItem> usableObjects;

    [Header("Percentage")]
    [SerializeField] private float percentageOfObject;
    [SerializeField] private float percentageOfType;

    private void Update()
    {
        if  (!done)
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

    private void GenerateItems()
    {
        Transform itemsParent;
        float chanceOfTypeObject;
        int objectIndex;
        foreach (RoomParameters room in rooms)
        {
            itemsParent = room.transform.GetChild(room.transform.childCount - 1);
            for (int i = 0; i < itemsParent.childCount; i++)
            {
                Transform transformItem = itemsParent.GetChild(i).GetComponent<Transform>();
                if (transformItem != null)
                {
                    chanceOfTypeObject = Random.Range(0f, 1f);
                    CollectableItem itemsSpwaned;
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
            }
        }
    }
}