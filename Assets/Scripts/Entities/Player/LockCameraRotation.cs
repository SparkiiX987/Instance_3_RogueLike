using System.Collections;
using UnityEngine;

public class LockCameraRotation : MonoBehaviour
{
    [SerializeField] private Transform entities;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    private Transform player;

    private void Start()
    {
        StartCoroutine(GetPlayer());
    }

    private IEnumerator GetPlayer()
    {
        while(!player)
        {
            if(entities.childCount > 0)
            { player = entities.GetChild(0); }

            if(!player)
            {
                yield return new WaitForEndOfFrame();
            }
        }
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
