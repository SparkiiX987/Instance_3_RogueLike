using System.Collections;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    private PickableObject item;
    [SerializeField] private int price;
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private Sprite inventorySprite;
    [SerializeField] private Sprite floorSprite;
    [SerializeField] private Sprite highlightedFloorSprite;
    [SerializeField] private float distance;
    public int itemType;
    private GameObject player;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitializeItem();
        StartCoroutine(GetPlayer());
    }

    public Sprite GetInventorySprite => inventorySprite;

    private void InitializeItem()
    {
        switch (itemType)
        {
            case 0:
                item = new SellableObject(price, itemName, description);
                break;
            case 1:
                item = new PepperSpray(price, itemName, description);
                break;
            case 2:
                item = new EmptyBottle(price, itemName, description);
                break;
            case 3:
                item = new MonsterCan(price, itemName, description);
                break;
            case 4:
                item = new WoodenPlank(price, itemName, description);
                break;
        }
    }

    public PickableObject Item => item;

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

        //print(Vector3.Distance(player.transform.position, transform.position));
        spriteRenderer.sprite = distance <= Vector3.Distance(player.transform.position, transform.position) ? floorSprite : highlightedFloorSprite;
    }
}
