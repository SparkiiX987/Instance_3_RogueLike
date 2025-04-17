using System.Collections;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public PickableObject item;
    [SerializeField] private Sprite inventorySprite;
    [SerializeField] private Sprite floorSprite;
    [SerializeField] private Sprite highlightedFloorSprite;
    [SerializeField] private float distance;
    private GameObject player;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(GetPlayer());
    }

    private IEnumerator GetPlayer()
    {
        while(player is null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if(player is not null)
            {
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        if(player is null) { return; }

        spriteRenderer.sprite = distance <= Vector3.Distance(player.transform.position, transform.position) ? highlightedFloorSprite : floorSprite;
    }
}
