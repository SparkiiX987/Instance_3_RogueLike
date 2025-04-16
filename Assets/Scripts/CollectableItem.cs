using System;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public PickableObject item;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite sprite;
    [SerializeField] Sprite highlightedSprite;
    [SerializeField] GameObject player;
    [SerializeField] float distance;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (distance <= Vector3.Distance(player.transform.position, transform.position))
        {
            spriteRenderer.sprite = highlightedSprite;
        }
        else
        {
            spriteRenderer.sprite = sprite;
        }
    }
}
