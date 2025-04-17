using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public PickableObject item;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite inventorySprite;
    [SerializeField] Sprite floorSprite;
    [SerializeField] Sprite highlightedFloorSprite;
    [SerializeField] float distance;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        spriteRenderer.sprite = distance <= Vector3.Distance(player.transform.position, transform.position) ? highlightedFloorSprite : floorSprite;
    }
}
