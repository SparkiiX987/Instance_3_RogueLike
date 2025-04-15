using System;
using UnityEngine;

public class LockCameraRotation : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
