using System.Collections.Generic;
using UnityEngine;

public class HighLightObject : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    [SerializeField] private float distance;
    [SerializeField] private Transform player;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (player is null) { return; }

        print(distance);
        spriteRenderer.sprite = distance <= Vector3.Distance(player.transform.position, transform.position) ? sprites[0] : sprites[1];
    }
}
